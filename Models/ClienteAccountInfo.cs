using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskIcosoftBackend.Models
{
    public class ClienteAccountInfo
    {
        [Key]
        public int IdClienteAccountInfo { get; set; } // Clave primaria

        [Required]
        [MaxLength(100)]
        public string Client { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(256)]
        public string Password { get; set; }

        [Required]
        [MaxLength(100)]
        public string AppPassword { get; set; }

        [Required]
        [MaxLength(100)]
        public string Vin { get; set; }

        [Required]
        public DateTime Date1 { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public bool Status { get; set; } // Estado activo/inactivo
        
       
    }
}