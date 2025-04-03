using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gymconnect_backend.Models;
using Microsoft.EntityFrameworkCore;
using TaskIcosoftBackend.Data;
using TaskIcosoftBackend.Dtos.Images;
using TaskIcosoftBackend.Models;

namespace TaskIcosoftBackend.Repository
{
    public class UserRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<UserRepository> _logger;
        private readonly ImageRepository _imageRepository; // Agregado el repository de imagen

        // Constructor modificado para inyectar ImageRepository
        public UserRepository(DataContext context, ILogger<UserRepository> logger, ImageRepository imageRepository)
        {
            _context = context;
            _logger = logger;
            _imageRepository = imageRepository; // Inicializar ImageRepository
        }

        // Obtener usuario por email
        public async Task<User?> GetUserByEmailAsync(string email)
        {
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Image)
                    .ThenInclude(i => i.ImageType)
                .FirstOrDefaultAsync(u => u.Email == email && u.Status);
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
        }

        // Obtener usuario por ID con roles e im�genes
        public async Task<User?> GetUserByIdAsync(int id)
        {
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Image)
                    .ThenInclude(i => i.ImageType)
                .FirstOrDefaultAsync(u => u.IdUser == id && u.Status); // Solo registros activos
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
        }

        // Obtener usuario por token de verificaci�n (usando un campo como ejemplo: `PasswordRecoverCode`)
        public async Task<User?> GetUserByTokenAsync(string token)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.PasswordRecoverCode == token);
        }
        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        // Eliminar usuario
        public async Task DeleteUserAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        // Listar todos los usuarios
        public async Task<List<User>> GetAllUsersWithRelationsAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Image)
                .ThenInclude(i => i.ImageType)
                .Where(u => u.Status) // Filtrar solo usuarios activos
                .ToListAsync();
        }



        // Obtener usuario por n�mero de tel�fono
        public async Task<User?> GetUserByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber && u.Status);
        }


        public async Task<User?> GetUserByActivationCodeAsync(string activationCode)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.PasswordRecoverCode == activationCode);
        }

        public async Task<User?> GetActiveUserByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Image)
                .ThenInclude(i => i.ImageType)
                .Where(u => u.IdUser == id && u.Status) // Filtra solo usuarios activos
                .FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetAllUsersWithRelationsAsyncInactive()
        {
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Image)
                .ThenInclude(i => i.ImageType)
                .Where(u => !u.Status) // Filtrar solo usuarios inactivos
                .ToListAsync();
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
        }
        public async Task<List<Session>> GetActiveSessionsByUserIdAsync(int userId)
        {
            return await _context.Sessions
                .Where(s => s.IdUser == userId && s.Status)
                .ToListAsync();
        }

        public async Task<List<User>> GetAllUsersWRole3Active()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Image)
                    .ThenInclude(i => i.ImageType)
                .Where(u => u.IdRole == 3 && u.Status)
                .ToListAsync();
        }

        public async Task<List<User>> GetAllUsersWRole3Inactive()
        {
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Image)
                    .ThenInclude(i => i.ImageType)
                .Where(u => u.IdRole == 3 && !u.Status)
                .ToListAsync();
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
        }

        public async Task<User?> GetByIdUsersWRole3Active(int id)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Image)
                    .ThenInclude(i => i.ImageType)
                .FirstOrDefaultAsync(u => u.IdUser == id && u.IdRole == 3 && u.Status);
        }

        public async Task<int> InsertImageAndUpdateUserAsync(int userId, CreateImageBase64RequestDto imageRequest)
        {
            try
            {
                string base64Image = imageRequest.Base64Image;

                if (string.IsNullOrEmpty(base64Image))
                {
                    _logger.LogError("La imagen en base64 está vacía.");
                    return -1;
                }

                int imageId = await _imageRepository.InsertImageAsync(base64Image);

                if (imageId == -1)
                {
                    _logger.LogError("Error al insertar la imagen.");
                    return -1;
                }

                var user = await _context.Users.FindAsync(userId);

                if (user == null)
                {
                    _logger.LogError("Usuario no encontrado con ID: {UserId}", userId);
                    return -1;
                }

                user.IdImage = imageId;
                user.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Imagen insertada y asociada correctamente al usuario con ID: {UserId}", userId);
                return imageId;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error en el servicio al insertar la imagen y asociarla al usuario: {Message}", ex.Message);
                return -1;
            }
        }

        public async Task<int> UpdateImageAndUpdateUserAsync(int userId, CreateImageBase64RequestDto imageRequest)
        {
            try
            {
                string base64Image = imageRequest.Base64Image;

                if (string.IsNullOrEmpty(base64Image))
                {
                    _logger.LogError("La imagen en base64 está vacía.");
                    return -1;
                }

                // Insertar la nueva imagen en la tabla Images
                int imageId = await _imageRepository.InsertImageAsync(base64Image);

                if (imageId == -1)
                {
                    _logger.LogError("Error al insertar la imagen.");
                    return -1;
                }

                // Obtener el usuario por ID
                var user = await _context.Users.FindAsync(userId);

                if (user == null)
                {
                    _logger.LogError("Usuario no encontrado con ID: {UserId}", userId);
                    return -1;
                }

                // Actualizar la imagen del usuario con el nuevo ID de imagen
                user.IdImage = imageId;
                user.UpdatedAt = DateTime.UtcNow;

                // Guardar los cambios en el usuario
                await _context.SaveChangesAsync();

                _logger.LogInformation("Imagen actualizada correctamente para el usuario con ID: {UserId}", userId);
                return imageId;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al actualizar la imagen del usuario: {Message}", ex.Message);
                return -1;
            }
        }
    }
}
