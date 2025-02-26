using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using TaskIcosoftBackend.Common;

namespace TaskIcosoftBackend.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Ejecuta la siguiente acción en el pipeline
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex); // Maneja la excepción
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // Determina el código de estado basado en el tipo de excepción
            var statusCode = ex switch
            {
                ArgumentNullException => (int)HttpStatusCode.BadRequest,
                InvalidOperationException => (int)HttpStatusCode.Conflict,
                _ => (int)HttpStatusCode.InternalServerError
            };

            // Crea una respuesta de error consistente
            var response = ApiResponse<string>.Error(ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            // Retorna la respuesta en formato JSON
            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}