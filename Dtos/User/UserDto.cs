using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskIcosoftBackend.Dtos.ImageDtos;
using TaskIcosoftBackend.Dtos.Role;

namespace TaskIcosoftBackend.Dtos.User
{
    public class UserDto
    {
        public int IdUser { get; set; }
        public int IdRole { get; set; }
        public RoleDto? Role { get; set; }
        public int? IdImage { get; set; }
        public ImageDto? Image { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FirstSurname { get; set; } = string.Empty;
        public string SecondSurname { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsVerified { get; set; } = false;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Status { get; set; } = false;
    }
}