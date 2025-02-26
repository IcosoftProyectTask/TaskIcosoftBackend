using System.ComponentModel.DataAnnotations;

namespace TaskIcosoftBackend.Dtos.Images
{
    public class UpdateImageBase64RequestDto
    {
        [Required(ErrorMessage = "La imagen en base 64 es necesaria.")]
        public string? UpdateBase64Image { get; set; }
    }
}
