using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gymconnect_backend.Models;
using Microsoft.EntityFrameworkCore;
using TaskIcosoftBackend.Custom;
using TaskIcosoftBackend.Data;

namespace TaskIcosoftBackend.Repository
{
    public class SessionRepository
    {
        private readonly DataContext _context;

        private readonly ILogger<SessionRepository> _logger;

        private readonly IConfiguration _configuration;

        private readonly Utils _utils;

        public SessionRepository(DataContext context, ILogger<SessionRepository> logger, IConfiguration configuration, Utils utils)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _utils = utils;
        }

        // Obtener sesión activa por usuario
        public async Task<Session?> GetActiveSessionByUserId(int userId)
        {
            var session = await _context.Sessions
                .Include(s => s.SessionType)
                .Include(s => s.User) // Asegura que incluyes al usuario si es necesario
                .FirstOrDefaultAsync(s => s.IdUser == userId && s.Status);

            if (session == null)
            {
                Console.WriteLine($"No se encontró una sesión activa para el usuario con ID: {userId}");
            }

            return session;
        }

        // Obtener sesión por token
        public async Task<Session?> GetSessionByTokenAsync(string sessionToken)
        {
            var session = await _context.Sessions
                .Include(s => s.User)
                .Include(s => s.SessionType)
                .FirstOrDefaultAsync(s => s.SessionToken == sessionToken && s.Status);

            if (session == null)
            {
                Console.WriteLine($"No se encontró una sesión activa con el token: {sessionToken}");
            }

            return session;
        }

        // Crear nueva sesión
        public async Task AddSessionAsync(Session session)
        {
            try
            {
                while (await _context.Sessions.AnyAsync(s => s.SessionToken == session.SessionToken))
                {
                    _logger.LogWarning("Token duplicado detectado. Generando un nuevo token.");
                    var user = await _context.Users.FindAsync(session.IdUser);
                    if (user == null)
                    {
                        throw new Exception($"Usuario con ID {session.IdUser} no encontrado.");
                    }
                    var audience = _configuration["Frontend:Url"];
                    if (string.IsNullOrEmpty(audience))
                    {
                        throw new ArgumentNullException(nameof(audience), "Frontend URL cannot be null or empty.");
                    }
                    session.SessionToken = _utils.GenerateJwtToken(user, audience);
                }

                _context.Sessions.Add(session);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Sesión creada exitosamente. IdUsuario: {IdUser}, IdSession: {IdSession}", session.IdUser, session.IdSession);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al guardar la sesión: {Message}", ex.Message);
                throw new DbUpdateException("Error al guardar la sesión en la base de datos.", ex);
            }
        }

        // Actualizar sesión
        public async Task UpdateSessionAsync(Session session)
        {
            try
            {
                _context.Sessions.Update(session);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Sesión actualizada para el usuario con ID: {session.IdUser}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar la sesión: {ex.Message}");
                throw;
            }
        }

        // Revocar todas las sesiones activas de un usuario
        public async Task RevokeAllSessionsByUserIdAsync(int userId)
        {
            var activeSessions = await _context.Sessions.Where(s => s.IdUser == userId && s.Status).ToListAsync();

            foreach (var session in activeSessions)
            {
                session.Status = false;
                session.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Todas las sesiones activas para el usuario {UserId} han sido revocadas.", userId);
        }

        // Método privado para realizar actualizaciones masivas de sesiones
        public async Task BulkUpdateSessionsAsync(IEnumerable<Session> sessions)
        {
            try
            {
                _context.Sessions.UpdateRange(sessions);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar sesiones: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Session>> GetActiveSessionsByUserIdAsync(int userId)
        {
            return await _context.Sessions
                .Where(s => s.IdUser == userId && s.Status)
                .ToListAsync();
        }
    }
}