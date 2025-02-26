using Microsoft.AspNetCore.Mvc;
using TaskIcosoftBackend.Service;
using TaskIcosoftBackend.Dtos.User;
using TaskIcosoftBackend.Models;
using TaskIcosoftBackend.Mappers;
using TaskIcosoftBackend.Custom;
using TaskIcosoftBackend.Common;
using System.Security.Principal;

namespace TaskIcosoftBackend.Controllers.Authorization
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;

        private readonly ILogger<AuthController> _logger;

        private readonly Utils _utils;


        public AuthController(UserService userService, ILogger<AuthController> logger, Utils utils)
        {
            _userService = userService;
            _logger = logger;
            _utils = utils;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto createUserDto)
        {
            _logger.LogInformation("Inicio del endpoint de registro para el usuario con email: {Email}", createUserDto.Email);

            try
            {
                if (createUserDto.Password != createUserDto.RepeatPassword)
                {
                    _logger.LogWarning("Las contraseñas no coinciden para el usuario {Email}", createUserDto.Email);
                    return BadRequest(ApiResponse<string>.Error("Las contraseñas no coinciden."));
                }

                _logger.LogInformation("Mapeando DTO a modelo para el usuario {Email}", createUserDto.Email);
                var user = UserMapper.ToModel(createUserDto);

                _logger.LogInformation("Llamando al servicio de registro para el usuario {Email}", createUserDto.Email);
                var createdUser = await _userService.RegisterUserAsync(user, createUserDto.Password, createUserDto.RepeatPassword);

                _logger.LogInformation("Mapeando el usuario registrado al DTO para el usuario {Email}", createUserDto.Email);
                var userDto = UserMapper.ToDto(createdUser);

                _logger.LogInformation("Usuario registrado exitosamente: {Email}", createUserDto.Email);
                return Ok(ApiResponse<UserDto>.Ok(userDto, "Usuario registrado con éxito. Revisa tu correo para activar tu cuenta."));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error de operación inválida durante el registro del usuario con email: {Email}", createUserDto.Email);
                return Conflict(ApiResponse<string>.Error(ex.Message));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Error de argumento durante el registro del usuario con email: {Email}", createUserDto.Email);
                return BadRequest(ApiResponse<string>.Error(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error general durante el registro del usuario con email: {Email}", createUserDto.Email);
                return StatusCode(500, ApiResponse<string>.Error("Ocurrió un error al registrar al usuario. " + ex.Message));
            }
        }



        [HttpPost("verify")]
        public async Task<IActionResult> VerifyActivationCode([FromBody] VerifyActivationCodeDto verifyActivationCodeDto)
        {
            try
            {
                if (string.IsNullOrEmpty(verifyActivationCodeDto.ActivationCode))
                {
                    return BadRequest(ApiResponse<string>.Error("El código de activación no puede estar vacío."));
                }

                var user = await _userService.GetUserByActivationCodeAsync(verifyActivationCodeDto.ActivationCode);
                if (user == null)
                {
                    return BadRequest(ApiResponse<string>.Error("Código de activación inválido o expirado."));
                }

                await _userService.ActivateUserAsync(user);
                return Ok(ApiResponse<string>.Ok("Cuenta activada con éxito."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error("Ocurrió un error al activar la cuenta. " + ex.Message));
            }
        }


        [HttpGet("resend-activation/{email}")]
        public async Task<IActionResult> ResendActivation(string email)
        {
            try
            {
                // Obtener usuario por email
                var user = await _userService.GetUserByEmailAsync(email);

                if (user == null)
                {
                    return NotFound(ApiResponse<string>.Error("Usuario no encontrado."));
                }

                if (user.IsVerified)
                {
                    return BadRequest(ApiResponse<string>.Error("La cuenta ya está activada."));
                }

                // Reenviar activación
                await _userService.ResendActivationAsync(user);

                // Respuesta exitosa
                return Ok(ApiResponse<string>.Ok(null, "Correo de activación reenviado con éxito."));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<string>.Error(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ApiResponse<string>.Error(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error("Ocurrió un error al reenviar el correo de activación. " + ex.Message));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto, [FromHeader(Name = "User-Agent")] string userAgent)
        {
            _logger.LogInformation("Inicio de sesión solicitado para el usuario: {Email}", loginDto.Email);

            try
            {
                // Detectar la dirección IP del usuario desde el contexto HTTP
                string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

                // Detectar el tipo de sesión basado en el encabezado "User-Agent"
                int sessionTypeId = userAgent.ToLower().Contains("mobile") ? 2 : 1; // 1: WebApp, 2: MobileApp

                _logger.LogInformation("Tipo de sesión detectado: {SessionTypeId} para el usuario: {Email}", sessionTypeId, loginDto.Email);

                // Llamar al servicio para manejar el inicio de sesión
                var loginResponse = await _userService.LoginUserAsync(
                    loginDto.Email,
                    loginDto.Password,
                    sessionTypeId,
                    ipAddress,
                    userAgent
                );

                _logger.LogInformation("Inicio de sesión exitoso para el usuario: {Email}", loginDto.Email);

                return Ok(ApiResponse<LoginResponseDto>.Ok(loginResponse, "Inicio de sesión exitoso."));
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Intento de inicio de sesión fallido: {Message}", ex.Message);
                return Unauthorized(ApiResponse<string>.Error(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado durante el inicio de sesión para el usuario: {Email}", loginDto.Email);
                return StatusCode(500, ApiResponse<string>.Error($"Error en el inicio de sesión: {ex.Message}"));
            }
        }


        // Refrescar token
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromHeader] string sessionToken)
        {
            try
            {
                var refreshResponse = await _userService.RefreshTokenAsync(sessionToken);
                return Ok(ApiResponse<LoginResponseDto>.Ok(refreshResponse, "Token actualizado con éxito."));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<string>.Error(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error("Error al refrescar el token: " + ex.Message));
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromHeader] string sessionToken)
        {
            if (string.IsNullOrWhiteSpace(sessionToken))
            {
                _logger.LogWarning("Token de sesión no proporcionado.");
                return BadRequest(ApiResponse<string>.Error("Debe proporcionar un token de sesión válido."));
            }

            try
            {
                await _userService.LogoutUserAsync(sessionToken);
                _logger.LogInformation("Sesión cerrada exitosamente para el token proporcionado.");
                return Ok(ApiResponse<string>.Ok(null, "Sesión cerrada con éxito."));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Error al cerrar sesión: {Message}", ex.Message);
                return NotFound(ApiResponse<string>.Error(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado durante el cierre de sesión.");
                return StatusCode(500, ApiResponse<string>.Error("Error al cerrar la sesión: " + ex.Message));
            }
        }
    }
}
