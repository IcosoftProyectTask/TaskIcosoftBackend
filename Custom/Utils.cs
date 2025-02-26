using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using TaskIcosoftBackend.Models;

namespace TaskIcosoftBackend.Custom
{
    public class Utils
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<Utils> _logger;

        private readonly IDistributedCache _cache;

        public Utils(IConfiguration configuration, ILogger<Utils> logger, IDistributedCache cache)
        {
            _configuration = configuration;
            _logger = logger;
            _cache = cache;

            _logger.LogInformation("Utils inicializado correctamente.");
        }

        // Método para encriptar contraseñas usando SHA-256
        public static string EncriptPasswordSHA256(string password)
        {
            using var sha256Hash = SHA256.Create();
            var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        // Método para validar contraseñas
        public static bool ValidatePassword(string inputPassword, string hashedPassword)
        {
            var hashedInput = EncriptPasswordSHA256(inputPassword);
            return hashedInput == hashedPassword;
        }

        // Método para obtener la clave de seguridad decodificada
        private SymmetricSecurityKey GetSecurityKey()
        {
            string secretKey = _configuration["Jwt:Key"]!;
            if (string.IsNullOrEmpty(secretKey))
            {
                _logger.LogError("La clave JWT no está configurada.");
                throw new InvalidOperationException("La clave JWT no está configurada.");
            }

            try
            {
                var keyBytes = Convert.FromBase64String(secretKey);
                return new SymmetricSecurityKey(keyBytes);
            }
            catch (FormatException ex)
            {
                _logger.LogError(ex, "La clave JWT no está correctamente codificada en Base64.");
                throw new InvalidOperationException("La clave JWT no está correctamente codificada en Base64.", ex);
            }
        }

        // Método para generar JWT Token
        public string GenerateJwtToken(User user, string audience)
        {
            _logger.LogInformation("Generando token para el usuario ID: {UserId}, Email: {Email}", user.IdUser, user.Email);

            var securityKey = GetSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Verificar si el usuario tiene un rol asociado
            string userRole = user.Role?.RoleName ?? "User";

            if (string.IsNullOrEmpty(userRole))
            {
                _logger.LogWarning("El usuario con ID {UserId} no tiene un rol asociado. Asignando rol por defecto: User.", user.IdUser);
                userRole = "User"; // Asignar rol por defecto
            }

            // Generar un identificador único para el token
            var jti = Guid.NewGuid().ToString();

            // Incluir el JTI en los claims
            var userClaims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, userRole), // Usar el rol del usuario (o el rol por defecto)
        new Claim(JwtRegisteredClaimNames.Jti, jti)
    };

            // Crear el token con los claims y el JTI
            var jwtToken = new JwtSecurityToken(
                issuer: _configuration["Backend:Url"],
                audience: audience,
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials
            );

            string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            _logger.LogInformation("Token generado para el usuario {Email} con rol: {Role}", user.Email, userRole);
            return token;
        }

        // Método para generar un Refresh Token
        public static string GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        // Validación de JWT Token
        public ClaimsPrincipal? ValidateJwtToken(string token)
        {
            _logger.LogInformation("Iniciando validación del token JWT: {Token}", token);

            try
            {
                // Verificar si el token está revocado utilizando caché
                if (IsTokenRevokedAsync(token).Result)
                {
                    _logger.LogWarning("El token JWT ha sido revocado y no es válido: {Token}", token);
                    return null;
                }

                // Obtener la clave de seguridad
                var securityKey = GetSecurityKey();

                // Configurar los parámetros de validación
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Backend:Url"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Frontend:Url"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // Eliminar margen para expiración de token
                };

                // Validar el token
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                _logger.LogInformation("Token JWT validado con éxito: {Token}", token);
                return principal;
            }
            catch (SecurityTokenExpiredException ex)
            {
                _logger.LogWarning(ex, "El token JWT ha expirado: {Token} - {Message}", token, ex.Message);
                return null; // Token expirado
            }
            catch (SecurityTokenInvalidIssuerException ex)
            {
                _logger.LogError(ex, "El emisor del token JWT es inválido: {Message}", ex.Message);
                return null;
            }
            catch (SecurityTokenInvalidAudienceException ex)
            {
                _logger.LogError(ex, "La audiencia del token JWT es inválida: {Message}", ex.Message);
                return null;
            }
            catch (SecurityTokenInvalidSignatureException ex)
            {
                _logger.LogError(ex, "La firma del token JWT es inválida: {Message}", ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al validar el token JWT: {Message}", ex.Message);
                return null;
            }
        }

        // Método para generar códigos únicos
        public static string GenerateUniqueCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new char[6];
            using var rng = RandomNumberGenerator.Create();
            var randomBytes = new byte[6];
            rng.GetBytes(randomBytes);
            for (int i = 0; i < random.Length; i++)
            {
                random[i] = chars[randomBytes[i] % chars.Length];
            }
            return new string(random);
        }

        // Método para revocar un token
        public async Task<bool> RevokeTokenAsync(string token)
        {
            _logger.LogInformation("Intentando revocar token: {Token}", token);

            // Verificar si el token ya está revocado
            if (await IsTokenRevokedAsync(token))
            {
                _logger.LogWarning("El token ya ha sido revocado previamente.");
                return false; // Evitar procesamiento innecesario
            }

            // Validar el token antes de revocarlo
            var principal = ValidateJwtToken(token);
            if (principal == null)
            {
                _logger.LogWarning("No se puede revocar un token inválido.");
                return false;
            }

            // Persistir el token revocado en la base de datos o caché
            await PersistRevokedTokenAsync(token);

            _logger.LogInformation("Token revocado con éxito: {Token}", token);
            return true;
        }

        public static bool ValidatePasswordStrength(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("La contraseña es obligatoria.");
            }

            if (password.Length < 8)
            {
                throw new ArgumentException("La contraseña debe tener al menos 8 caracteres.");
            }

            if (password.Length > 28)
            {
                throw new ArgumentException("La contraseña debe tener como máximo 28 caracteres.");
            }

            if (!Regex.IsMatch(password, @"^(?=(?:.*[a-z]){2})(?=(?:.*[A-Z]){2})(?=(?:.*\d){2})(?=(?:.*[*.@_]){2})[a-zA-Z\d*.@_]{8,28}$"))
            {
                throw new ArgumentException("La contraseña debe incluir al menos 2 letras minúsculas, 2 letras mayúsculas, 2 números y 2 caracteres especiales permitidos (*, ., @, _).");
            }
            return true;
        }
        // Verificar si un token está revocado
        public async Task<bool> IsTokenRevokedAsync(string token)
        {
            _logger.LogInformation("Verificando si el token está revocado: {Token}", token);

            try
            {
                var cachedValue = await _cache.GetAsync($"revokedToken:{token}");
                return cachedValue != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar el estado del token en el caché: {Message}", ex.Message);
                return false; // En caso de error, considera que no está revocado
            }
        }

        private async Task PersistRevokedTokenAsync(string token)
        {
            // Persistir en caché "Redis" con una expiración igual al tiempo de vida del token
            var expiration = DateTime.UtcNow.AddMinutes(30); // Tiempo de expiración del token
            await _cache.SetAsync($"revokedToken:{token}", Encoding.UTF8.GetBytes("revoked"), new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = expiration
            });
        }

        public static string GenerateTemporaryPassword()
        {
            const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
            const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "0123456789";
            const string specialChars = "*.@_";

            Random random = new Random();
            StringBuilder password = new StringBuilder();

            password.Append(new string(Enumerable.Repeat(lowerCase, 2).Select(s => s[random.Next(s.Length)]).ToArray()));
            password.Append(new string(Enumerable.Repeat(upperCase, 2).Select(s => s[random.Next(s.Length)]).ToArray()));
            password.Append(new string(Enumerable.Repeat(digits, 2).Select(s => s[random.Next(s.Length)]).ToArray()));
            password.Append(new string(Enumerable.Repeat(specialChars, 2).Select(s => s[random.Next(s.Length)]).ToArray()));

            string allChars = lowerCase + upperCase + digits + specialChars;
            password.Append(new string(Enumerable.Repeat(allChars, 8).Select(s => s[random.Next(s.Length)]).ToArray()));

            return new string(password.ToString().OrderBy(c => random.Next()).ToArray());
        }
    }
    
}