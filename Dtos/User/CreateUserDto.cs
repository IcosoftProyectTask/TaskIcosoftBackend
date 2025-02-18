using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskIcosoftBackend.Dtos.User
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MinLength(3, ErrorMessage = "El nombre debe tener al menos 3 caracteres.")]
        [MaxLength(12, ErrorMessage = "El nombre debe tener como máximo 12 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        [MinLength(3, ErrorMessage = "El apellido debe tener al menos 3 caracteres.")]
        [MaxLength(15, ErrorMessage = "El apellido debe tener como máximo 15 caracteres.")]
        public string FirstSurname { get; set; } = string.Empty;

        [Required(ErrorMessage = "El segundo apellido es obligatorio.")]
        [MinLength(3, ErrorMessage = "El apellido debe tener al menos 3 caracteres.")]
        [MaxLength(15, ErrorMessage = "El apellido debe tener como máximo 15 caracteres.")]
        public string SecondSurname { get; set; } = string.Empty;

        [Required(ErrorMessage = "El número de teléfono es obligatorio.")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El número de teléfono debe tener exactamente 8 dígitos.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        [MaxLength(28, ErrorMessage = "La contraseña debe tener como máximo 28 caracteres.")]
        [RegularExpression(
            @"^(?=(?:.*[a-z]){2})(?=(?:.*[A-Z]){2})(?=(?:.*\d){2})(?=(?:.*[*.@_]){2})[a-zA-Z\d*.@_]{8,28}$",
            ErrorMessage = "La contraseña debe incluir al menos 2 letras minúsculas, 2 letras mayúsculas, 2 números y 2 caracteres especiales permitidos (*, ., @, _)."
        )]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "La repetición de la contraseña es obligatoria.")]
        public string RepeatPassword { get; set; } = string.Empty;

       
    }
}