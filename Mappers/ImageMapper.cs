using System;
using TaskIcosoftBackend.Dtos.ImageDtos;
using TaskIcosoftBackend.Dtos.ImageType;
using TaskIcosoftBackend.Models;

namespace TaskIcosoftBackend.Mappers
{
    public static class ImageMapper
    {
        /// <summary>
        /// Convierte un modelo de Image a un DTO de ImageDto.
        /// </summary>
        public static ImageDto ToDto(this Image image)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image), "La imagen no puede ser nula.");

            return new ImageDto
            {
                IdImage = image.IdImage,
                IdImageType = image.IdImageType,
                ImageType = image.ImageType != null
                    ? new ImageTypeDto
                    {
                        IdImageType = image.ImageType.IdImageType,
                        ImageTypeName = image.ImageType.ImageTypeName,
                        CreatedAt = image.ImageType.CreatedAt,
                        UpdatedAt = image.ImageType.UpdatedAt
                    }
                    : null, // Manejo seguro de nulos
                Base64Image = image.Base64Image,
                CreatedAt = image.CreatedAt,
                UpdatedAt = image.UpdatedAt
            };
        }

        /// <summary>
        /// Convierte un DTO de ImageDto a un modelo de Image.
        /// </summary>
        public static Image ToModel(this ImageDto imageDto)
        {
            if (imageDto == null)
                throw new ArgumentNullException(nameof(imageDto), "El DTO de la imagen no puede ser nulo.");

            return new Image
            {
                IdImage = imageDto.IdImage,
                IdImageType = imageDto.IdImageType,
                Base64Image = imageDto.Base64Image,
                CreatedAt = imageDto.CreatedAt,
                UpdatedAt = imageDto.UpdatedAt
            };
        }

        /*
        public static GymImage ToUserFromCreateDto(this CreateImageRequestDto createImageRequestDto)
        {
            return new GymImage
            {
                IdImageType = createImageRequestDto.IdImageType,

            };
        }
        */
    }
}
