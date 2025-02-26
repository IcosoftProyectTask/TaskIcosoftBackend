using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskIcosoftBackend.Dtos.ImageType;

namespace TaskIcosoftBackend.Dtos.ImageDtos
{
    public class ImageDto
    {
        public int IdImage { get; set; } // Clave primaria

        public int IdImageType { get; set; } // FK a ImageType

        // Propiedad para incluir el objeto relacionado de ImageType
        public ImageTypeDto? ImageType { get; set; }

        public string Base64Image { get; set; } = string.Empty; // Contenido de la imagen en base64

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}