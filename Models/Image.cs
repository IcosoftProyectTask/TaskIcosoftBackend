using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaskIcosoftBackend.Models
{
    public class Image
    {
        [Key]
        public int IdImage { get; set; }

        [Required]
        public int IdImageType { get; set; } // FK a ImageType

        [ForeignKey("IdImageType")]
        public ImageType? ImageType { get; set; } // Propiedad de navegación a ImageType

        [Required]
        [MaxLength(int.MaxValue)] //Al valor máximo que soporte la base de datos para la cadena de string de la imagen.
        public string Base64Image { get; set; } = string.Empty; // Contenido de la imagen en base64

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}