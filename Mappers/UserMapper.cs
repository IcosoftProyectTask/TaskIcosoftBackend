using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskIcosoftBackend.Custom;
using TaskIcosoftBackend.Dtos.CommentsTask;
using TaskIcosoftBackend.Dtos.User;
using TaskIcosoftBackend.Models;

namespace TaskIcosoftBackend.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToDto(User user)
        {
            Console.WriteLine($"Mapeando modelo a DTO: {user.Email}");
            return new UserDto
            {
                IdUser = user.IdUser,
                IdRole = user.IdRole,
                Role = user.Role != null ? RoleMapper.ToDto(user.Role) : null,
                IdImage = user.IdImage,
                Image = user.Image != null ? ImageMapper.ToDto(user.Image) : null,
                Name = user.Name,
                FirstSurname = user.FirstSurname,
                SecondSurname = user.SecondSurname,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                IsVerified = user.IsVerified,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                Status = user.Status
            };
        }

               public static User ToModel(CreateUserDto createUserDto)
        {
            Console.WriteLine($"Mapeando DTO a modelo: {createUserDto.Email}");
            return new User
            {
                IdRole = 2, // Rol predeterminado (Admin).
                Name = createUserDto.Name,
                FirstSurname = createUserDto.FirstSurname,
                SecondSurname = createUserDto.SecondSurname,
                PhoneNumber = createUserDto.PhoneNumber,
                Email = createUserDto.Email,
                Password = createUserDto.Password, // Contraseña sin hashear (hash antes de guardar).
                IsVerified = false, // Valor predeterminado.
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Status = true // Activado por defecto.
            };
        }
        public static User ToModel(AdminCreateUserRequest request, string temporaryPassword)
        {
            return new User
            {
                IdRole = 3, 
                Name = request.Name,
                FirstSurname = request.FirstSurname,
                SecondSurname = request.SecondSurname,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Password = Utils.EncriptPasswordSHA256(temporaryPassword),
                IsVerified = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Status = true
            };
        }

        public static void MapUpdateFcmTokenRequestToUser(UpdateFcmTokenRequestDto request, User user)
        {
            user.FcmToken = request.FcmToken;
        }

        public static UserBasicDto ToBasicDto(User user)
        {
            return new UserBasicDto
            {
                Id = user.IdUser,
                Name = user.Name,
                Avatar = user.Image?.Base64Image // Asumiendo que el avatar es la imagen en Base64
            };
        }
    }
}