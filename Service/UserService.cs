using TaskIcosoftBackend.Custom;
using TaskIcosoftBackend.Dtos.User;
using TaskIcosoftBackend.Mappers;
using TaskIcosoftBackend.Models;
using TaskIcosoftBackend.Repository;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using gymconnect_backend.Models;
using TaskIcosoftBackend.Dtos.Images;

namespace TaskIcosoftBackend.Service
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly SessionRepository _sessionRepository;
        private readonly ImageRepository _imageRepository;


        private readonly ILogger<UserService> _logger;

        private readonly Utils _utils;


        public UserService(UserRepository userRepository, EmailService emailService, IConfiguration configuration, SessionRepository sessionRepository, Utils utils, ILogger<UserService> logger, ImageRepository imageRepository)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _configuration = configuration;
            _sessionRepository = sessionRepository;
            _utils = utils;
            _logger = logger;
            _imageRepository = imageRepository;
        }

        public async Task<User> RegisterUserAsync(User user, string password, string repeatPassword)
        {
            _logger.LogInformation("Inicio del registro para el usuario con email: {Email}", user.Email);

            try
            {
                var existingUser = await _userRepository.GetUserByEmailAsync(user.Email);
                if (existingUser != null)
                {
                    _logger.LogWarning("El correo electrónico {Email} ya está registrado", user.Email);
                    throw new InvalidOperationException("El correo electrónico ya está registrado.");
                }

                _logger.LogInformation("Validación de correo completada");

                var existingPhoneUser = await _userRepository.GetUserByPhoneNumberAsync(user.PhoneNumber);
                if (existingPhoneUser != null)
                {
                    _logger.LogWarning("El número de teléfono {PhoneNumber} ya está registrado", user.PhoneNumber);
                    throw new InvalidOperationException("El número de teléfono ya está registrado.");
                }

                if (password != repeatPassword)
                {
                    _logger.LogWarning("Las contraseñas no coinciden para el usuario {Email}", user.Email);
                    throw new ArgumentException("Las contraseñas no coinciden.");
                }

                _logger.LogInformation("Validación de contraseñas completada");

                user.Password = Utils.EncriptPasswordSHA256(password);
                user.CreatedAt = DateTime.UtcNow;

                await _userRepository.AddUserAsync(user);
                _logger.LogInformation("Usuario {Email} agregado a la base de datos", user.Email);

                var token = Utils.GenerateUniqueCode();
                user.PasswordRecoverCode = token;
                await _userRepository.UpdateUserAsync(user);
                _logger.LogInformation("Token generado y usuario {Email} actualizado", user.Email);

                await SendActivationEmail(user, token);
                _logger.LogInformation("Correo de activación enviado al usuario {Email}", user.Email);

                // Recargar el usuario con las relaciones
                var registeredUser = await _userRepository.GetUserByIdAsync(user.IdUser);
                if (registeredUser == null)
                {
                    throw new Exception("No se pudo cargar el usuario registrado.");
                }

                return registeredUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el registro del usuario con email: {Email}", user.Email);
                throw;
            }
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                throw new KeyNotFoundException($"Usuario no encontrado con el correo: {email} proporcionado.");
            }
            return user;
        }

        public async Task<User?> GetUserByTokenAsync(string token)
        {
            // Obtener usuario por token
            var user = await _userRepository.GetUserByTokenAsync(token);

            if (user == null)
            {
                Console.WriteLine("Usuario no encontrado para el token proporcionado.");
                return null; // Usuario no encontrado
            }

            // Verificar si el token coincide
            if (user.PasswordRecoverCode != token)
            {
                Console.WriteLine($"El token proporcionado ({token}) no coincide con el almacenado ({user.PasswordRecoverCode}).");
                return null; // Token no coincide
            }

            // Validar tiempo de expiración del token
            var expirationTime = user.UpdatedAt.AddMinutes(30); // Ajusta el tiempo de expiración si es necesario
            if (expirationTime < DateTime.UtcNow)
            {
                Console.WriteLine($"Token expirado. Fecha de expiración: {expirationTime}, Fecha actual: {DateTime.UtcNow}");
                return null; // Token expirado
            }

            Console.WriteLine($"Token válido para el usuario {user.Email}. Fecha de expiración: {expirationTime}");
            return user; // Token válido
        }

        public async Task ResendActivationAsync(User user)
        {
            if (user.IsVerified)
            {
                throw new InvalidOperationException("La cuenta ya está activada.");
            }

            // Generar un nuevo token
            var token = Utils.GenerateUniqueCode();
            user.PasswordRecoverCode = token;
            user.UpdatedAt = DateTime.UtcNow; // Actualizar la fecha de modificación para validar el token nuevo generado.

            // Actualizar usuario en la base de datos para poder comparar el token
            await _userRepository.UpdateUserAsync(user);

            // Enviar email con el nuevo token
            await SendActivationEmail(user, token);
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Iniciando búsqueda del usuario con ID: {Id}", id);

                // Buscar el usuario en el repositorio con la condición de que esté activo
                var user = await _userRepository.GetActiveUserByIdAsync(id);

                if (user == null)
                {
                    _logger.LogWarning("Usuario no encontrado o inactivo con ID: {Id}", id);
                    throw new KeyNotFoundException($"Usuario no encontrado o inactivo con el ID: {id} proporcionado.");
                }

                _logger.LogInformation("Usuario encontrado con ID: {Id}", id);

                // Convierte el modelo User a UserDto antes de devolverlo
                return UserMapper.ToDto(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetUserByIdAsync para el ID: {Id}", id);
                throw;
            }
        }
        public async Task UpdateUserAsync(int id, UpdateUserDto updateUserDto)
        {
            // Verificar si el usuario existe
            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                throw new KeyNotFoundException("No se puede actualizar porque el usuario no existe.");
            }

            // Validar que el número de teléfono no esté registrado en otro usuario
            var existingPhoneUser = await _userRepository.GetUserByPhoneNumberAsync(updateUserDto.PhoneNumber);
            if (existingPhoneUser != null && existingPhoneUser.IdUser != id)
            {
                _logger.LogWarning("El número de teléfono {PhoneNumber} ya está registrado por otro usuario.", updateUserDto.PhoneNumber);
                throw new InvalidOperationException("El número de teléfono ya está registrado por otro usuario.");
            }

            // Actualizar campos del usuario
            existingUser.Name = updateUserDto.Name;
            existingUser.FirstSurname = updateUserDto.FirstSurname;
            existingUser.SecondSurname = updateUserDto.SecondSurname;
            existingUser.PhoneNumber = updateUserDto.PhoneNumber;

            await _userRepository.UpdateUserAsync(existingUser);
        }

        public async Task DeleteUserAsync(int id)
        {
            _logger.LogInformation("Iniciando proceso de eliminación lógica para el usuario con ID: {Id}", id);

            // Buscar el usuario en la base de datos
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("No se encontró el usuario con ID: {Id}. No se puede eliminar.", id);
                throw new KeyNotFoundException("No se puede eliminar porque el usuario no existe.");
            }

            // Borrado logico del usuario cambiando el status a false.
            user.Status = false;
            user.UpdatedAt = DateTime.UtcNow;

            // Guardar los cambios en la base de datos
            await _userRepository.UpdateUserAsync(user);

            _logger.LogInformation("El usuario con ID: {Id} ha sido marcado como inactivo.", id);
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            try
            {
                _logger.LogInformation("Iniciando obtención de todos los usuarios activos.");

                // Obtener todos los usuarios activos con sus relaciones relevantes
                var users = await _userRepository.GetAllUsersWithRelationsAsync();

                if (users == null || !users.Any())
                {
                    _logger.LogWarning("No se encontraron usuarios activos.");
                    return new List<UserDto>();
                }

                // Filtrar usuarios activos
                var activeUsers = users.Where(user => user.Status).ToList();

                if (!activeUsers.Any())
                {
                    _logger.LogWarning("No se encontraron usuarios activos después del filtro.");
                    return new List<UserDto>();
                }

                // Mapear la lista de usuarios activos a UserDto
                return activeUsers.Select(UserMapper.ToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de usuarios activos.");
                throw;
            }
        }

        public async Task<bool> UpdateFcmTokenAsync(UpdateFcmTokenRequestDto request)
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                return false;
            }

            UserMapper.MapUpdateFcmTokenRequestToUser(request, user);
            await _userRepository.UpdateUserAsync(user);

            return true;
        }

        public async Task<List<UserDto>> GetAllInactiveUsersAsync()
        {
            try
            {
                _logger.LogInformation("Iniciando obtención de todos los usuarios inactivos.");

                // Obtener todos los usuarios inactivos con sus relaciones relevantes
                var users = await _userRepository.GetAllUsersWithRelationsAsyncInactive();

                if (users == null || !users.Any())
                {
                    _logger.LogWarning("No se encontraron usuarios inactivos.");
                    return new List<UserDto>();
                }

                // Mapear la lista de usuarios inactivos a UserDto
                return users.Select(UserMapper.ToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de usuarios inactivos.");
                throw;
            }
        }

        public async Task ActivateUserAsync(User user)
        {
            user.IsVerified = true;
            user.PasswordRecoverCode = null; // Limpiar el token
            user.UpdatedAt = DateTime.UtcNow;

            // Actualizar usuario en la base de datos
            await _userRepository.UpdateUserAsync(user);
            Console.WriteLine($"Usuario {user.Email} activado correctamente.");
        }

        private async Task SendActivationEmail(User user, string token)
        {
            var verificationUrl = $"{_configuration["Frontend:Url"]}/auth/verify-account";
            var resendActivationUrl = $"{_configuration["Backend:Url"]}/api/auth/resend-activation/{user.Email}";
            var subject = "Activa tu cuenta - IcosoftTask";

            var body = $@"
    <div style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
        <div style='max-width: 600px; margin: auto; background: #ffffff; padding: 20px; border-radius: 10px; box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);'>
            <h2 style='color: #007BFF; text-align: center;'>¡Bienvenido a IcosoftTask!</h2>
            <p style='font-size: 16px; color: #333; text-align: center;'>Gracias por registrarte en <strong>Icosoft</strong>.</p>
            <p style='font-size: 16px; color: #333; text-align: center;'>Tu código de activación es:</p>
            <div style='text-align: center; background: #007BFF; color: #ffffff; padding: 15px; font-size: 20px; font-weight: bold; border-radius: 5px;'>
                {token}
            </div>
            <p style='font-size: 16px; color: #333; text-align: center;'>Ingresa este código en la página de activación para activar tu cuenta.</p>
            <div style='text-align: center; margin: 20px 0;'>
                <a href='{verificationUrl}' style='background: #007BFF; color: #ffffff; padding: 10px 20px; text-decoration: none; font-size: 16px; border-radius: 5px;'>Activar cuenta</a>
            </div>
            <p style='font-size: 14px; color: #666; text-align: center;'>El código expira en 30 minutos. Activa tu cuenta pronto o solicita una nueva activación.</p>
            <p style='font-size: 14px; color: #666; text-align: center;'>Si necesitas un nuevo código de activación, haz clic aquí:</p>
            <div style='text-align: center; margin: 20px 0;'>
                <a href='{resendActivationUrl}' style='color: #007BFF; font-weight: bold; text-decoration: none;'>Reenviar activación de cuenta</a>
            </div>
        </div>
    </div>";

            Console.WriteLine($"Enviando correo a: {user.Email} con código de activación: {token}");
            await _emailService.SendEmailAsync(user.Email, subject, body);
        }

        public async Task<User?> GetUserByActivationCodeAsync(string activationCode)
        {
            var user = await _userRepository.GetUserByActivationCodeAsync(activationCode);
            if (user == null)
            {
                Console.WriteLine("Usuario no encontrado para el código de activación proporcionado.");
                return null;
            }

            // Tiempo de expiración del código
            var expirationTime = user.UpdatedAt.AddMinutes(30);
            if (expirationTime < DateTime.UtcNow)
            {
                Console.WriteLine($"Código de activación expirado. Fecha de expiración: {expirationTime}, Fecha actual: {DateTime.UtcNow}");
                return null;
            }

            Console.WriteLine($"Código de activación válido para el usuario {user.Email}. Fecha de expiración: {expirationTime}");
            return user;
        }

        ///////////////////----------------------------LOGIN Y JWT----------------------------////////////////////
        public async Task<LoginResponseDto> LoginUserAsync(string email, string password, int sessionTypeId, string ipAddress, string deviceInfo)
        {
            _logger.LogInformation("Iniciando el proceso de inicio de sesión para el usuario con email: {Email}", email);

            try
            {
                // Buscar el usuario en la base de datos
                var user = await _userRepository.GetUserByEmailAsync(email);
                if (user == null)
                {
                    _logger.LogWarning("Inicio de sesión fallido. Usuario no encontrado: {Email}", email);
                    throw new UnauthorizedAccessException("Credenciales inválidas.");
                }

                // Validar si el usuario está activo
                if (!user.Status)
                {
                    _logger.LogWarning("Inicio de sesión fallido. Usuario inactivo: {Email}", email);
                    throw new UnauthorizedAccessException("La cuenta está inactiva. Por favor, contacte al administrador.");
                }

                // Validar si el usuario está verificado
                if (!user.IsVerified)
                {
                    _logger.LogWarning("Inicio de sesión fallido. Usuario no verificado: {Email}", email);
                    throw new UnauthorizedAccessException("La cuenta no está verificada. Por favor, revise su correo electrónico.");
                }

                // Validar la contraseña
                if (!Utils.ValidatePassword(password, user.Password))
                {
                    _logger.LogWarning("Inicio de sesión fallido. Contraseña inválida para el usuario: {Email}", email);
                    throw new UnauthorizedAccessException("Credenciales inválidas.");
                }

                // Revocar todas las sesiones activas del usuario
                _logger.LogInformation("Revocando sesiones activas para el usuario: {UserId}", user.IdUser);
                await _sessionRepository.RevokeAllSessionsByUserIdAsync(user.IdUser);

                // Determinar la audiencia (web o móvil)
                var audience = sessionTypeId == 1
                    ? _configuration["Frontend:Url"] ?? throw new InvalidOperationException("La URL del frontend web no está configurada.")
                    : _configuration.GetSection("FrontendMovil:Urls").Get<string[]>()?[0] ?? throw new InvalidOperationException("La URL del frontend móvil no está configurada.");

                // Generar el JWT Token
                var token = _utils.GenerateJwtToken(user, audience);
                var expirationDate = DateTime.UtcNow.AddMinutes(30);

                // Crear la nueva sesión
                var session = new Session
                {
                    IdUser = user.IdUser,
                    IdSessionType = sessionTypeId,
                    SessionToken = token,
                    IpAddress = ipAddress,
                    DeviceInfo = deviceInfo,
                    ExpirationSessionDate = expirationDate,
                    LastActivityDate = DateTime.UtcNow,
                    Status = true
                };

                // Guardar la sesión en la base de datos
                _logger.LogInformation("Creando una nueva sesión para el usuario: {UserId}", user.IdUser);
                await _sessionRepository.AddSessionAsync(session);

                _logger.LogInformation("Inicio de sesión exitoso para el usuario: {Email}", email);

                // Devolver la respuesta con el token y los datos relevantes
                return new LoginResponseDto
                {
                    Token = token,
                    ExpirationDate = expirationDate,
                    SessionType = sessionTypeId == 1 ? "WebApp" : "MovilApp"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al iniciar sesión para el usuario: {Email}", email);
                throw;
            }
        }

        // Cerrar sesión del usuario
        public async Task LogoutUserAsync(string sessionToken)
        {
            // Obtener la sesión por token
            var session = await _sessionRepository.GetSessionByTokenAsync(sessionToken);
            if (session == null)
            {
                _logger.LogWarning("No se encontró una sesión activa para el token proporcionado.");
                throw new KeyNotFoundException("Sesión no encontrada.");
            }

            // Revocar el token
            var isRevoked = await _utils.RevokeTokenAsync(sessionToken);
            if (isRevoked)
            {
                _logger.LogInformation("Token revocado exitosamente: {Token}", sessionToken);
            }
            else
            {
                _logger.LogWarning("El token ya había sido revocado: {Token}", sessionToken);
            }

            // Marcar la sesión como inactiva
            session.Status = false;
            session.UpdatedAt = DateTime.UtcNow;
            await _sessionRepository.UpdateSessionAsync(session);

            _logger.LogInformation("Sesión cerrada exitosamente para el token: {Token}", sessionToken);
        }

        // Refresco de token
        public async Task<LoginResponseDto> RefreshTokenAsync(string sessionToken)
        {
            var session = await _sessionRepository.GetSessionByTokenAsync(sessionToken);
            if (session == null || !session.Status || session.ExpirationSessionDate < DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("La sesión ha expirado o es inválida.");
            }

            // Validar que la propiedad User no sea null
            if (session.User == null)
            {
                throw new InvalidOperationException("La sesión no tiene un usuario asociado.");
            }

            // Determinar la audiencia según el tipo de sesión
            var audience = session.IdSessionType == 1
                ? _configuration["Frontend:Url"] ?? throw new InvalidOperationException("La URL del frontend web no está configurada.")
                : _configuration["FrontendMobile:Url"] ?? throw new InvalidOperationException("La URL del frontend móvil no está configurada.");

            var newToken = _utils.GenerateJwtToken(session.User, audience);
            session.SessionToken = newToken;
            session.LastActivityDate = DateTime.UtcNow;
            session.ExpirationSessionDate = DateTime.UtcNow.AddMinutes(30);

            await _sessionRepository.UpdateSessionAsync(session);

            return new LoginResponseDto
            {
                Token = newToken,
                ExpirationDate = session.ExpirationSessionDate,
                SessionType = session.SessionType?.SessionTypeName ?? "Desconocido"
            };
        }
        public async Task ChangePasswordAsync(ClaimsPrincipal userPrincipal, string oldPassword, string newPassword, string repeatPassword)
        {
            var userIdClaim = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                _logger.LogWarning("El usuario no tiene un ID válido en el token.");
                throw new UnauthorizedAccessException("Usuario no autenticado.");
            }

            _logger.LogInformation("Iniciando cambio de contraseña para el usuario con ID {UserId}.", userId);

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("Usuario no encontrado.");
            }

            if (!Utils.ValidatePassword(oldPassword, user.Password))
            {
                throw new UnauthorizedAccessException("La contraseña anterior es incorrecta.");
            }

            if (newPassword != repeatPassword)
            {
                throw new ArgumentException("Las contraseñas no coinciden.");
            }

            Utils.ValidatePasswordStrength(newPassword);

            if (Utils.ValidatePassword(newPassword, user.Password))
            {
                throw new InvalidOperationException("La nueva contraseña no puede ser igual a la anterior.");
            }

            user.Password = Utils.EncriptPasswordSHA256(newPassword);
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateUserAsync(user);

            _logger.LogInformation("Revocando todas las sesiones activas del usuario con ID: {UserId}", userId);
            var sessions = await _sessionRepository.GetActiveSessionsByUserIdAsync(userId);

            foreach (var session in sessions)
            {
                await _utils.RevokeTokenAsync(session.SessionToken);
                session.Status = false;
                session.UpdatedAt = DateTime.UtcNow;
                await _sessionRepository.UpdateSessionAsync(session);
            }

            _logger.LogInformation("Cambio de contraseña completado y todas las sesiones cerradas para el usuario con ID: {UserId}", userId);
        }

        public async Task RecoverPasswordByEmailAsync(RecoverPasswordRequest request)
        {
            _logger.LogInformation("Iniciando proceso de recuperación de contraseña para: {Email}", request.Email);

            // Validar que el usuario existe y está activo
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null || !user.Status)
            {
                _logger.LogWarning("Usuario no encontrado o inactivo para el email: {Email}", request.Email);
                throw new ArgumentException("El correo proporcionado no está registrado o el usuario no está activo.");
            }

            // Generar una contraseña temporal
            string temporaryPassword = Utils.GenerateTemporaryPassword();

            // Validar la fortaleza de la contraseña generada
            Utils.ValidatePasswordStrength(temporaryPassword);

            // Asignar y encriptar la contraseña temporal
            string hashedTemporaryPassword = Utils.EncriptPasswordSHA256(temporaryPassword);
            user.Password = hashedTemporaryPassword; // Actualiza la contraseña para el inicio de sesión
            user.PasswordRecoverCode = hashedTemporaryPassword; // Almacena el mismo hash como código de recuperación
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateUserAsync(user);
            _logger.LogInformation("Contraseña temporal generada y guardada para el usuario: {Email}", request.Email);

            // Crear el cuerpo del correo
            string emailBody = BuildRecoveryEmailBody(temporaryPassword);

            try
            {
                // Enviar el correo
                await _emailService.SendEmailAsync(user.Email, "Recuperación de contraseña - GymConnect", emailBody);
                _logger.LogInformation("Correo de recuperación enviado con éxito a {Email}", user.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar correo de recuperación a {Email}", user.Email);
                throw new InvalidOperationException("No se pudo enviar el correo de recuperación. Por favor, inténtalo nuevamente más tarde.");
            }
        }
        private static string BuildRecoveryEmailBody(string temporaryPassword)
        {
            return $@"
    <div style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
        <div style='max-width: 600px; margin: auto; background: #ffffff; padding: 20px; border-radius: 10px; box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);'>
            <h2 style='color: #007BFF; text-align: center;'>Recuperación de Contraseña</h2>
            <p style='font-size: 16px; color: #333; text-align: center;'>Hola, saludos de parte de <strong>IcosoftTask</strong>, esperamos que estés bien.</p>
            <p style='font-size: 16px; color: #333; text-align: center;'>Hemos recibido una solicitud de recuperación de contraseña y hemos generado esta contraseña temporal para ti:</p>
            <div style='text-align: center; background: #007BFF; color: #ffffff; padding: 15px; font-size: 20px; font-weight: bold; border-radius: 5px;'>
                {temporaryPassword}
            </div>
            <p style='font-size: 16px; color: #333; text-align: center;'>Esta contraseña temporal expirará en <strong>24 horas</strong>. Te recomendamos actualizar tu contraseña dentro del sistema lo más pronto posible para no perder el acceso.</p>
            <p style='font-size: 14px; color: #666; text-align: center;'>Gracias,<br><strong>El equipo de IcosoftTask</strong></p>
        </div>
    </div>";
        }

        public async Task AdminCreateUserAsync(AdminCreateUserRequest request)
        {
            _logger.LogInformation("Iniciando creación de usuario por parte del administrador para: {Email}", request.Email);

            var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("El correo electrónico {Email} ya está registrado", request.Email);
                throw new ArgumentException("El correo electrónico ya está registrado.");
            }

            var existingPhoneUser = await _userRepository.GetUserByPhoneNumberAsync(request.PhoneNumber);
            if (existingPhoneUser != null)
            {
                _logger.LogWarning("El número de teléfono {PhoneNumber} ya está registrado", request.PhoneNumber);
                throw new ArgumentException("El número de teléfono ya está registrado.");
            }

            string temporaryPassword = Utils.GenerateTemporaryPassword();

            Utils.ValidatePasswordStrength(temporaryPassword);

            var user = UserMapper.ToModel(request, temporaryPassword);

            await _userRepository.AddUserAsync(user);
            _logger.LogInformation("Usuario creado exitosamente por el administrador: {Email}", request.Email);

            string emailBody = BuildAdminCreationEmailBody(temporaryPassword);

            try
            {
                await _emailService.SendEmailAsync(user.Email, "Creación de cuenta - IcosoftTask", emailBody);
                _logger.LogInformation("Correo enviado exitosamente a {Email}", user.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar el correo de creación al usuario: {Email}", user.Email);
                throw new InvalidOperationException("No se pudo enviar el correo de creación. Por favor, inténtalo nuevamente más tarde.");
            }
        }

        private static string BuildAdminCreationEmailBody(string temporaryPassword)
        {
            return $@"
                <p>Hola de parte de Icosoft, esperamos que te encuentres bien,</p>
                <p>Le has solicitado la creación de tu cuenta a uno de nuestros administradores. Te hemos generado esta contraseña temporal para tu acceso al sistema:</p>
                <p><strong>{temporaryPassword}</strong></p>
                <p><strong>Esta contraseña temporal expira a las 24 horas</strong> de haberse enviado este correo. Por favor, cuando inicies sesión, actualiza tu contraseña dentro del sistema lo antes posible para no perder tu acceso.</p>
                <p>Gracias,<br>El equipo de Icosoft</p>";
        }

        // Obtener todos los usuarios con rol 3 (User) activos
        public async Task<List<UserDto>> GetAllUsersWithRole3ActiveAsync()
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los usuarios con rol 3 activos.");

                var users = await _userRepository.GetAllUsersWRole3Active();

                if (!users.Any())
                {
                    _logger.LogWarning("No se encontraron usuarios activos con rol 3.");
                    throw new KeyNotFoundException("No se encontraron usuarios activos con rol 3.");
                }

                _logger.LogInformation("Se encontraron {Count} usuarios activos con rol 3.", users.Count);

                return users.Select(UserMapper.ToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetAllUsersWithRole3ActiveAsync.");
                throw;
            }
        }

        // Obtener todos los usuarios con rol 3 (User) inactivos
        public async Task<List<UserDto>> GetAllUsersWithRole3InactiveAsync()
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los usuarios con rol 3 inactivos.");

                var users = await _userRepository.GetAllUsersWRole3Inactive();

                if (!users.Any())
                {
                    _logger.LogWarning("No se encontraron usuarios inactivos con rol 3.");
                    throw new KeyNotFoundException("No se encontraron usuarios inactivos con rol 3.");
                }

                _logger.LogInformation("Se encontraron {Count} usuarios inactivos con rol 3.", users.Count);

                return users.Select(UserMapper.ToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetAllUsersWithRole3InactiveAsync.");
                throw;
            }
        }

        // Obtener un usuario por ID si tiene rol 3 (User) y está activo
        public async Task<UserDto> GetUserByIdWithRole3ActiveAsync(int id)
        {
            try
            {
                _logger.LogInformation("Buscando usuario con ID {Id} y rol 3 activo.", id);

                var user = await _userRepository.GetByIdUsersWRole3Active(id);

                if (user == null)
                {
                    _logger.LogWarning("Usuario con ID {Id} y rol 3 activo no encontrado.", id);
                    throw new KeyNotFoundException($"Usuario con ID {id} y rol 3 activo no encontrado.");
                }

                _logger.LogInformation("Usuario con ID {Id} y rol 3 activo encontrado.", id);

                return UserMapper.ToDto(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetUserByIdWithRole3ActiveAsync para el ID: {Id}", id);
                throw;
            }
        }

        public async Task<int> InsertImageAndUpdateUserAsync(int userId, CreateImageBase64RequestDto imageRequest)
        {
            try
            {
                int imageId = await _userRepository.InsertImageAndUpdateUserAsync(userId, imageRequest);

                if (imageId == -1)
                {
                    _logger.LogError("Error al insertar la imagen y asociarla al usuario.");
                    return -1;
                }

                _logger.LogInformation("Imagen insertada y asociada correctamente al usuario con ID: {UserId}", userId);
                return imageId;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al insertar la imagen y asociarla al usuario: {Message}", ex.Message);
                return -1;
            }
        }

        public async Task<int> UpdateImageAndUpdateUserAsync(int userId, UpdateImageBase64RequestDto imageRequest)
        {
            try
            {
                var createUpdateImageRequest = new CreateImageBase64RequestDto
                {
                    Base64Image = imageRequest.UpdateBase64Image
                };

                int imageId = await _userRepository.UpdateImageAndUpdateUserAsync(userId, createUpdateImageRequest);

                if (imageId == -1)
                {
                    _logger.LogError("Error al actualizar la imagen y asociarla al usuario.");
                    return -1;
                }

                _logger.LogInformation("Imagen actualizada y asociada correctamente al usuario con ID: {UserId}", userId);
                return imageId;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al actualizar la imagen y asociarla al usuario: {Message}", ex.Message);
                return -1;
            }
        }
    }
}
