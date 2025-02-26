using Microsoft.AspNetCore.Mvc;
using TaskIcosoftBackend.Service;
using TaskIcosoftBackend.Dtos.User;
using TaskIcosoftBackend.Common;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TaskIcosoftBackend.Dtos.Images;

namespace TaskIcosoftBackend.Controllers.User
{
    [Authorize] 
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(UserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                _logger.LogInformation("Solicitud para obtener todos los usuarios activos.");

                var users = await _userService.GetAllUsersAsync();

                if (!users.Any())
                {
                    _logger.LogWarning("No se encontraron usuarios activos.");
                    return NotFound(ApiResponse<string>.Error("No se encontraron usuarios activos."));
                }

                _logger.LogInformation("Usuarios activos obtenidos: {Count}", users.Count);
                return Ok(ApiResponse<IEnumerable<UserDto>>.Ok(users, "Usuarios activos obtenidos con éxito."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de usuarios activos.");
                return StatusCode(500, ApiResponse<string>.Error($"Error al obtener la lista de usuarios: {ex.Message}"));
            }
        }

        [HttpGet("getAllInactiveUsers")]
        public async Task<IActionResult> GetAllInactiveUsers()
        {
            try
            {
                _logger.LogInformation("Solicitud para obtener todos los usuarios inactivos.");

                var users = await _userService.GetAllInactiveUsersAsync();

                if (!users.Any())
                {
                    _logger.LogWarning("No se encontraron usuarios inactivos.");
                    return NotFound(ApiResponse<string>.Error("No se encontraron usuarios inactivos."));
                }

                _logger.LogInformation("Usuarios inactivos obtenidos: {Count}", users.Count);
                return Ok(ApiResponse<IEnumerable<UserDto>>.Ok(users, "Usuarios inactivos obtenidos con éxito."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de usuarios inactivos.");
                return StatusCode(500, ApiResponse<string>.Error($"Error al obtener la lista de usuarios inactivos: {ex.Message}"));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                _logger.LogInformation("Solicitud para obtener usuario con ID: {Id}", id);

                var userDto = await _userService.GetUserByIdAsync(id);

                return Ok(ApiResponse<UserDto>.Ok(userDto, "Usuario obtenido con éxito."));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Usuario no encontrado: {Message}", ex.Message);
                return NotFound(ApiResponse<string>.Error(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuario con ID: {Id}", id);
                return StatusCode(500, ApiResponse<string>.Error($"Error al obtener el usuario: {ex.Message}"));
            }
        }

        [HttpPost("update-fcm-token")]
        public async Task<IActionResult> UpdateFcmToken([FromBody] UpdateFcmTokenRequestDto request)
        {
            var result = await _userService.UpdateFcmTokenAsync(request);
            if (!result)
            {
                return NotFound("Usuario no encontrado");
            }
            return Ok(ApiResponse<UpdateFcmTokenRequestDto>.Ok(null,"Token actualizado con éxito."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                _logger.LogInformation("Solicitud para eliminar el usuario con ID: {Id}", id);

                await _userService.DeleteUserAsync(id);

                _logger.LogInformation("Usuario con ID {Id} eliminado con éxito.", id);
                return Ok(ApiResponse<string>.Ok(null, "Usuario eliminado (marcado como inactivo) con éxito."));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Intento de eliminación fallido. Usuario con ID {Id} no encontrado.", id);
                return NotFound(ApiResponse<string>.Error(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al eliminar el usuario con ID {Id}.", id);
                return StatusCode(500, ApiResponse<string>.Error("Error interno del servidor."));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Datos de entrada inválidos para la actualización del usuario con ID {Id}.", id);
                return BadRequest(ApiResponse<string>.Error("Datos de entrada inválidos."));
            }

            try
            {
                _logger.LogInformation("Iniciando la actualización del usuario con ID {Id}.", id);

                await _userService.UpdateUserAsync(id, updateUserDto);

                _logger.LogInformation("Usuario con ID {Id} actualizado con éxito.", id);
                return Ok(ApiResponse<string>.Ok(null, "Usuario actualizado con éxito."));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Intento de actualización fallido. Usuario con ID {Id} no encontrado.", id);
                return NotFound(ApiResponse<string>.Error(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Validación fallida para el usuario con ID {Id}. Detalle: {Message}", id, ex.Message);
                return BadRequest(ApiResponse<string>.Error(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar el usuario con ID {Id}.", id);
                return StatusCode(500, ApiResponse<string>.Error("Error interno del servidor."));
            }
        }

        [HttpPut("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] UpdatePasswordUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Datos inválidos para el cambio de contraseña.");
                return BadRequest(ApiResponse<string>.Error("Datos inválidos para el cambio de contraseña."));
            }

            try
            {
                _logger.LogInformation("Solicitud de cambio de contraseña iniciada.");

                // Uso del servicio para cambiar la contraseña
                await _userService.ChangePasswordAsync(User, request.OldPassword, request.Password, request.RepeatPassword);

                _logger.LogInformation("Contraseña cambiada con éxito. Todas las sesiones activas han sido revocadas.");

                return Ok(ApiResponse<string>.Ok(null, "Contraseña cambiada con éxito. Debe volver a iniciar sesión."));
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Error de autenticación: {Message}", ex.Message);
                return Unauthorized(ApiResponse<string>.Error(ex.Message));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Error de validación: {Message}", ex.Message);
                return BadRequest(ApiResponse<string>.Error(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Error de operación: {Message}", ex.Message);
                return BadRequest(ApiResponse<string>.Error(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado durante el cambio de contraseña.");
                return StatusCode(500, ApiResponse<string>.Error("Error interno del servidor."));
            }
        }

        [HttpPost("recover-password")]
        [AllowAnonymous] // Permitir acceso sin autenticación
        public async Task<IActionResult> RecoverPassword([FromBody] RecoverPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Datos inválidos en la solicitud de recuperación de contraseña para el correo: {Email}", request.Email);
                return BadRequest(ApiResponse<string>.Error("Datos inválidos. Verifica el correo electrónico ingresado."));
            }

            try
            {
                _logger.LogInformation("Procesando recuperación de contraseña para el email: {Email}", request.Email);

                await _userService.RecoverPasswordByEmailAsync(request);

                _logger.LogInformation("Recuperación de contraseña procesada con éxito para el email: {Email}", request.Email);
                return Ok(ApiResponse<string>.Ok(null, "Se ha enviado un correo con la contraseña temporal."));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Error en la recuperación de contraseña para el email: {Email}. Detalles: {Message}", request.Email, ex.Message);
                return BadRequest(ApiResponse<string>.Error(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al procesar la recuperación de contraseña para el email: {Email}", request.Email);
                return StatusCode(500, ApiResponse<string>.Error("Error interno del servidor."));
            }
        }

        [HttpPost("admin-create-user")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> AdminCreateUser([FromBody] AdminCreateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Datos inválidos en la solicitud de creación de usuario por parte del administrador.");
                return BadRequest(ApiResponse<string>.Error("Datos inválidos. Verifica la información ingresada."));
            }

            try
            {
                _logger.LogInformation("Procesando creación de usuario por parte del administrador para el email: {Email}", request.Email);

                await _userService.AdminCreateUserAsync(request);

                _logger.LogInformation("Usuario creado exitosamente por el administrador: {Email}", request.Email);
                return Ok(ApiResponse<string>.Ok(null, "Usuario creado exitosamente."));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Validación fallida en la creación de usuario por parte del administrador: {Message}", ex.Message);
                return BadRequest(ApiResponse<string>.Error(ex.Message));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Error en la creación de usuario por parte del administrador: {Message}", ex.Message);
                return BadRequest(ApiResponse<string>.Error(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear usuario por parte del administrador.");
                return StatusCode(500, ApiResponse<string>.Error("Error interno del servidor."));
            }
        }

        // Obtener todos los usuarios con rol 3 activos
        [HttpGet("getAllRole3/active")]
        public async Task<IActionResult> GetAllUsersWithRole3Active()
        {
            try
            {
                _logger.LogInformation("Solicitud para obtener todos los usuarios con rol 3 activos.");

                var users = await _userService.GetAllUsersWithRole3ActiveAsync();

                return Ok(ApiResponse<List<UserDto>>.Ok(users, "Usuarios activos con rol 3 obtenidos con éxito."));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("No se encontraron usuarios activos con rol 3: {Message}", ex.Message);
                return NotFound(ApiResponse<string>.Error(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuarios activos con rol 3.");
                return StatusCode(500, ApiResponse<string>.Error($"Error al obtener usuarios: {ex.Message}"));
            }
        }

        // Obtener todos los usuarios con rol 3 inactivos
        [HttpGet("getAllRole3/inactive")]
        public async Task<IActionResult> GetAllUsersWithRole3Inactive()
        {
            try
            {
                _logger.LogInformation("Solicitud para obtener todos los usuarios con rol 3 inactivos.");

                var users = await _userService.GetAllUsersWithRole3InactiveAsync();

                return Ok(ApiResponse<List<UserDto>>.Ok(users, "Usuarios inactivos con rol 3 obtenidos con éxito."));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("No se encontraron usuarios inactivos con rol 3: {Message}", ex.Message);
                return NotFound(ApiResponse<string>.Error(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuarios inactivos con rol 3.");
                return StatusCode(500, ApiResponse<string>.Error($"Error al obtener usuarios: {ex.Message}"));
            }
        }

        // Obtener un usuario por ID si tiene rol 3 y está activo
        [HttpGet("getByIdRole3/active/{id}")]
        public async Task<IActionResult> GetUserByIdWithRole3Active(int id)
        {
            try
            {
                _logger.LogInformation("Solicitud para obtener usuario con ID {Id} y rol 3 activo.", id);

                var userDto = await _userService.GetUserByIdWithRole3ActiveAsync(id);

                return Ok(ApiResponse<UserDto>.Ok(userDto, "Usuario activo con rol 3 obtenido con éxito."));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Usuario con ID {Id} no encontrado: {Message}", id, ex.Message);
                return NotFound(ApiResponse<string>.Error(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuario con ID {Id} y rol 3 activo.", id);
                return StatusCode(500, ApiResponse<string>.Error($"Error al obtener usuario: {ex.Message}"));
            }
        }

        [HttpPost("insert-image/{userId}")]
        public async Task<ActionResult<ApiResponse<int>>> InsertImageAndUpdateUserAsync(int userId, [FromBody] CreateImageBase64RequestDto imageRequest)
        {
            if (imageRequest == null || string.IsNullOrEmpty(imageRequest.Base64Image))
            {
                return BadRequest(ApiResponse<int>.Error("La imagen en base64 no puede ser vacía."));
            }

            // Verificar si el Base64 es válido
            if (!IsValidBase64(imageRequest.Base64Image))
            {
                return BadRequest(ApiResponse<int>.Error("La cadena proporcionada no es una imagen en base64 válida."));
            }

            try
            {
                int imageId = await _userService.InsertImageAndUpdateUserAsync(userId, imageRequest);

                if (imageId == -1)
                {
                    return BadRequest(ApiResponse<int>.Error("Error al insertar la imagen o asociarla al usuario."));
                }

                return Ok(ApiResponse<int>.Ok(imageId, "Imagen insertada y asociada correctamente al usuario."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<int>.Error($"Error interno del servidor: {ex.Message}"));
            }
        }

        // Método para validar si la cadena es un Base64 válido
        private bool IsValidBase64(string base64)
        {
            if (string.IsNullOrEmpty(base64))
                return false;

            // Eliminar el prefijo "data:image/png;base64," si está presente en la imagen en base 64 para poder insetarla
            if (base64.StartsWith("data:image", StringComparison.OrdinalIgnoreCase))
            {
                var base64Data = base64.Substring(base64.IndexOf("base64,", StringComparison.OrdinalIgnoreCase) + 7); 
                base64 = base64Data;
            }

            // Comprobar si la cadena tiene un formato Base64 válido
            try
            {
                Convert.FromBase64String(base64); 
                return true;
            }
            catch
            {
                return false;
            }
        }

        [HttpPatch("update-image/{userId}")]
        public async Task<ActionResult<ApiResponse<int>>> UpdateImageAndUpdateUserAsync(int userId, [FromBody] UpdateImageBase64RequestDto imageRequest)
        {
            if (imageRequest == null || string.IsNullOrEmpty(imageRequest.UpdateBase64Image))
            {
                return BadRequest(ApiResponse<int>.Error("La imagen en base64 no puede ser vacía."));
            }

            // Verificar si el Base64 es válido
            if (!IsValidBase64(imageRequest.UpdateBase64Image))
            {
                return BadRequest(ApiResponse<int>.Error("La cadena proporcionada no es una imagen en base64 válida."));
            }

            try
            {
                // Llamar al servicio para actualizar la imagen y asociarla al usuario
                int imageId = await _userService.UpdateImageAndUpdateUserAsync(userId, imageRequest);

                if (imageId == -1)
                {
                    return BadRequest(ApiResponse<int>.Error("Error al actualizar la imagen o asociarla al usuario."));
                }

                return Ok(ApiResponse<int>.Ok(imageId, "Imagen actualizada y asociada correctamente al usuario."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<int>.Error($"Error interno del servidor: {ex.Message}"));
            }
        }
    }
}
