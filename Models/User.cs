using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using gymconnect_backend.Models;

namespace TaskIcosoftBackend.Models
{
    public class User
    {
        [Key]
        public int IdUser { get; set; } // Clave primaria

        [Required]
        public int IdRole { get; set; } // FK a Role

        [ForeignKey("IdRole")]
        public Role? Role { get; set; } // Relación con la tabla Role

        [Required]
        [MaxLength(int.MaxValue)] //Al valor máximo que soporte la base de datos para la cadena de string de la imagen.
        public string Base64Image { get; set; } = string.Empty; // Contenido de la imagen en base64

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty; // Nombre del usuario

        [Required]
        [MaxLength(100)]
        public string FirstSurname { get; set; } = string.Empty; // Primer apellido

        [Required]
        [MaxLength(100)]
        public string SecondSurname { get; set; } = string.Empty; // Segundo apellido

        [Required]
        [Phone]
        [MaxLength(15)]
        public string PhoneNumber { get; set; } = string.Empty; // Teléfono

        [Required]
        [MaxLength(15)]
        public string IdCart { get; set; } = string.Empty;

        public Boolean IsVerified { get; set; } = false; // Verificación de correo de usuario

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty; // Email

        [Required]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        [MaxLength(256)] // Para almacenar la contraseña encriptada
        public string Password { get; set; } = string.Empty; // Contraseña

        public string? PasswordRecoverCode { get; set; } = string.Empty; // Código de recuperación de contraseña
        public string? FcmToken { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool Status { get; set; } // Estado activo/inactivo
    }
}