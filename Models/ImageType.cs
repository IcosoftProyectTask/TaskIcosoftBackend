using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskIcosoftBackend.Models
{
    public class ImageType
    {
        [Key]
        public int IdImageType { get; set; }

        [Required]
        [MaxLength(50)]
        public string ImageTypeName { get; set; } = string.Empty; // Tipo de imagen (por ejemplo, "Imagen", "Gif")

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public bool Status { get; set; } // Estado (activo/inactivo)
    }
}