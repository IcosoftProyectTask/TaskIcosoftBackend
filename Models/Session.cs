using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskIcosoftBackend.Models;

namespace gymconnect_backend.Models
{
    public class Session
    {
        [Key]
        public int IdSession { get; set; } // Clave primaria

        [Required]
        public int IdUser { get; set; } // FK a User

        [ForeignKey("IdUser")]
        public User? User { get; set; } // Relación con User

        [Required]
        public int IdSessionType { get; set; }

        [ForeignKey("IdSessionType")]
        public SessionType? SessionType { get; set; }

        [Required]
        public string SessionToken { get; set; } = string.Empty; // Token de sesión único, sin límite de longitud

        [Required]
        [MaxLength(50)]
        public string? IpAddress { get; set; } = string.Empty; // Dirección IP del usuario

        [Required]
        [MaxLength(500)]
        public string DeviceInfo { get; set; } = string.Empty; // Información sobre el dispositivo

        [Required]
        public DateTime ExpirationSessionDate { get; set; } // Fecha de expiración

        [Required]
        public DateTime LastActivityDate { get; set; } // Fecha de última actividad

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public bool Status { get; set; } // Estado de la sesión (activa/inactiva)
    }
}
