
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gymconnect_backend.Models
{
    public class Role
    {
        [Key]
        public int IdRole { get; set; } // Clave primaria

        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; } = string.Empty; // Nombre del rol (ej. Admin, User)

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public bool Status { get; set; } // Estado activo/inactivo
    }
}