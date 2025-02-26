using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskIcosoftBackend.Dtos.Role
{
    public class RoleDto
    {
        public int IdRole { get; set; } // Clave primaria
        public string RoleName { get; set; } = string.Empty; 

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool Status { get; set; } // Estado activo/inactivo
    }
}