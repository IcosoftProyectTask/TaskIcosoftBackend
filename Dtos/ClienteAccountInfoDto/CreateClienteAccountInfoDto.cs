using System;
using System.ComponentModel.DataAnnotations;

namespace TaskIcosoftBackend.Dtos.ClienteAccountInfoDto
{
    public class CreateClienteAccountInfoDto
    {

        /*
        [Required(ErrorMessage = "El nombre del cliente es obligatorio.")]
        [MinLength(3, ErrorMessage = "El nombre del cliente debe tener al menos 3 caracteres.")]
        [MaxLength(100, ErrorMessage = "El nombre del cliente debe tener como máximo 100 caracteres.")]
        */
        public string Client { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        [MaxLength(100, ErrorMessage = "El correo electrónico debe tener como máximo 100 caracteres.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        [MaxLength(256, ErrorMessage = "La contraseña debe tener como máximo 256 caracteres.")]
        public string Password { get; set; } = string.Empty;

        /*
        [Required(ErrorMessage = "La contraseña de la aplicación es obligatoria.")]
        [MinLength(6, ErrorMessage = "La contraseña de la aplicación debe tener al menos 6 caracteres.")]
        [MaxLength(100, ErrorMessage = "La contraseña de la aplicación debe tener como máximo 100 caracteres.")]
        */
        public string AppPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "El VIN es obligatorio.")]
        [MinLength(6, ErrorMessage = "El VIN debe tener al menos 6 caracteres.")]
        [MaxLength(100, ErrorMessage = "El VIN debe tener como máximo 100 caracteres.")]
        public string Vin { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        public DateTime Date1 { get; set; }
    }
}