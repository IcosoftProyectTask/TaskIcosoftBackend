using System.ComponentModel.DataAnnotations;

namespace TaskIcosoftBackend.Dtos.Images
{
    public class CreateImageBase64RequestDto
    {
        [Required(ErrorMessage = "La imagen en base 64 es necesaria.")]
        public string? Base64Image { get; set; }
    }
}
