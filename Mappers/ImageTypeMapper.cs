using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskIcosoftBackend.Dtos.ImageType;
using TaskIcosoftBackend.Models;

namespace TaskIcosoftBackend.Mappers
{
    public static class ImageTypeMapper
    {
        public static ImageTypeDto ToDto(ImageType imageType)
        {
            if (imageType == null)
                throw new ArgumentNullException(nameof(imageType), "El tipo de imagen no puede ser nulo.");

            return new ImageTypeDto
            {
                IdImageType = imageType.IdImageType,
                ImageTypeName = imageType.ImageTypeName,
                CreatedAt = imageType.CreatedAt,
                UpdatedAt = imageType.UpdatedAt
            };
        }

    }
}