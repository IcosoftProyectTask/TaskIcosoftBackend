using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskIcosoftBackend.Dtos.RemoteDto
{
    public class UpdateRemoteDto
    {
        [Required(ErrorMessage = "El nombre del cliente es obligatorio.")]
        [MinLength(3, ErrorMessage = "El nombre del cliente debe tener al menos 3 caracteres.")]
        [MaxLength(100, ErrorMessage = "El nombre del cliente debe tener como máximo 100 caracteres.")]
        public string Customer { get; set; } = string.Empty;

        [Required(ErrorMessage = "El terminal es obligatorio.")]
        [MinLength(3, ErrorMessage = "El terminal debe tener al menos 3 caracteres.")]
        [MaxLength(100, ErrorMessage = "El terminal debe tener como máximo 100 caracteres.")]
        public string Terminal { get; set; } = string.Empty;

        [Required(ErrorMessage = "El software es obligatorio.")]
        [MinLength(2, ErrorMessage = "El software debe tener al menos 2 caracteres.")]
        [MaxLength(100, ErrorMessage = "El software debe tener como máximo 100 caracteres.")]
        public string Software { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección IP es obligatoria.")]
        [RegularExpression(@"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$", ErrorMessage = "La dirección IP no tiene un formato válido.")]
        [MaxLength(100, ErrorMessage = "La dirección IP debe tener como máximo 100 caracteres.")]
        public string IpAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "El usuario es obligatorio.")]
        [MinLength(3, ErrorMessage = "El usuario debe tener al menos 3 caracteres.")]
        [MaxLength(100, ErrorMessage = "El usuario debe tener como máximo 100 caracteres.")]
        public string User { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MaxLength(200, ErrorMessage = "La contraseña debe tener como máximo 100 caracteres.")]
        public string Password { get; set; } = string.Empty;

    }
}