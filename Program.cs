using TaskIcosoftBackend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication;
using System.Text;
using TaskIcosoftBackend.Models;
using TaskIcosoftBackend.Custom;
using TaskIcosoftBackend.Service;
using TaskIcosoftBackend.Middleware;
using TaskIcosoftBackend.Repository;
using System.Security.Cryptography;
using System.Security.Claims;
using gymconnect_backend.Logic.Validation.Rules;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;



var builder = WebApplication.CreateBuilder(args);

// Declare jwtKey variable
string jwtKey = builder.Configuration["Jwt:Key"] ?? string.Empty;
if (string.IsNullOrWhiteSpace(jwtKey))
    throw new InvalidOperationException("La clave JWT no está configurada.");

try
{
    // Ensure jwtKey is valid Base64
    Convert.FromBase64String(jwtKey);
}
catch (FormatException)
{
    throw new InvalidOperationException("La clave JWT no está correctamente codificada en Base64.");
}

// Configuración de la conexión a la base de datos
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro de servicios y utilidades
builder.Services.AddSingleton<Utils>();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:ConnectionString"];
    options.InstanceName = builder.Configuration["Redis:InstanceName"];
});

// Otros servicios.
builder.Services.AddSingleton<EmailService>();
builder.Services.AddScoped<SessionRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ImageRepository>();
builder.Services.AddScoped<CompanyService>();
builder.Services.AddScoped<CompanyRepository>();
builder.Services.AddScoped<CompanyEmployeeService>();
builder.Services.AddScoped<CompanyEmployeeRepository>();






builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;

    config.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = async context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogError(context.Exception, "Error de autenticación");

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            var result = System.Text.Json.JsonSerializer.Serialize(new { message = "Error de autenticación. Token inválido o expirado." });
            await context.Response.WriteAsync(result);
        },
        OnTokenValidated = async context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            var utils = context.HttpContext.RequestServices.GetRequiredService<Utils>();

            // Obtener el token
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Validar si el token está revocado
            if (await utils.IsTokenRevokedAsync(token))
            {
                logger.LogWarning("Intento de acceso con un token revocado: {Token}", token);
                context.Fail("Token revocado. Por favor, inicie sesión nuevamente.");
                return;
            }

            // Extraer información del token
            var claimsPrincipal = context.Principal;
            var userId = claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = claimsPrincipal?.FindFirst(ClaimTypes.Email)?.Value;

            logger.LogInformation("Token validado exitosamente para UserID: {UserId}, Email: {Email}, Issuer: {Issuer}, Audience: {Audience}",
                userId, email, context.Options.TokenValidationParameters.ValidIssuer, context.Options.TokenValidationParameters.ValidAudiences);

            await Task.CompletedTask;
        },
        OnChallenge = async context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogWarning("Token no proporcionado o inválido. Desafío iniciado.");

            context.HandleResponse();
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            var result = System.Text.Json.JsonSerializer.Serialize(new { message = "Acceso denegado. Token inválido o ausente." });
            await context.Response.WriteAsync(result);
        }
    };

    // Obtener valores de configuración
    var jwtKey = builder.Configuration["Jwt:Key"];
    var issuer = builder.Configuration["Backend:Url"];
    var frontendUrls = builder.Configuration.GetSection("FrontendMovil:Urls").Get<string[]>() ?? Array.Empty<string>();
    var frontendUrl = builder.Configuration["Frontend:Url"];

    if (string.IsNullOrWhiteSpace(jwtKey))
    {
        throw new InvalidOperationException("JWT key is not configured or is empty.");
    }

    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtKey)),

        ValidateIssuer = true,
        ValidIssuer = issuer,

        ValidateAudience = true,
        ValidAudiences = frontendUrls.Append(frontendUrl), // Permite múltiples clientes

        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Configuración de servicios adicionales
ConfigureServices(builder.Services);

var app = builder.Build();

// Configuración del pipeline HTTP
ConfigureMiddleware(app);

// Ejecutar la aplicación principal
await RunAppAsync(app);

void ConfigureServices(IServiceCollection services)
{
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "Ingrese el token JWT en el formato 'Bearer {token}'"
        });

        c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigins", policy =>
        {
            var frontendUrl = builder.Configuration["Frontend:Url"];
            var frontendMovilUrls = builder.Configuration.GetSection("FrontendMovil:Urls").Get<string[]>();
            var backendLocalUrl = builder.Configuration["Backend:Url"];
            var backendPublicUrl = builder.Configuration["Backend:PublicUrl"];
            var backendNetworkUrl = builder.Configuration["Backend:LocalNetworkUrl"];
            var backendAlternativeIps = builder.Configuration.GetSection("Backend:AlternativeIPs").Get<string[]>();
            var backendLocalAlternativeIps = builder.Configuration.GetSection("Backend:LocalAlternativeNetworkUrl").Get<string[]>();

            var origins = new List<string>();

            if (!string.IsNullOrWhiteSpace(frontendUrl)) origins.Add(frontendUrl);
            if (frontendMovilUrls != null) origins.AddRange(frontendMovilUrls);
            if (!string.IsNullOrWhiteSpace(backendLocalUrl)) origins.Add(backendLocalUrl);
            if (!string.IsNullOrWhiteSpace(backendPublicUrl)) origins.Add(backendPublicUrl);
            if (!string.IsNullOrWhiteSpace(backendNetworkUrl)) origins.Add(backendNetworkUrl);
            if (backendAlternativeIps != null) origins.AddRange(backendAlternativeIps);
            if (backendLocalAlternativeIps != null) origins.AddRange(backendLocalAlternativeIps);

            policy.WithOrigins(origins.ToArray())
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
    });

    services.AddDistributedMemoryCache();
    services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

    services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = null; // Elimina el seguimiento de referencias
        options.JsonSerializerOptions.MaxDepth = 200; // Ajusta la profundidad según sea necesario
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull; // Opcional
    });

}

void ConfigureMiddleware(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseSession();  
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseCors("AllowSpecificOrigins");  // Aplicar la política CORS definida.
    app.UseMiddleware<GlobalExceptionMiddleware>();
    app.MapControllers();
}

async Task RunAppAsync(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<DataContext>();
            Console.WriteLine("Aplicando migraciones...");
            await context.Database.MigrateAsync();
            Console.WriteLine("Migraciones aplicadas con éxito.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error durante migraciones: {ex.Message}");
            throw;
        }
    }

    await app.RunAsync();
}
