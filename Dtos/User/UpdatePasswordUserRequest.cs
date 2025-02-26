using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
namespace TaskIcosoftBackend.Dtos.User
{
    public class UpdatePasswordUserRequest
    {
        // Solicitar al usuario la contraseña anterior de su cuenta para validar y permitir el cambio de contraseña.
        [Required(ErrorMessage = "La contraseña anterior es obligatoria para el cambio de contraseña.")]
        public string OldPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        [MaxLength(28, ErrorMessage = "La contraseña debe tener como máximo 28 caracteres.")]
        [RegularExpression(
            @"^(?=(?:.*[a-z]){2})(?=(?:.*[A-Z]){2})(?=(?:.*\d){2})(?=(?:.*[*.@_]){2})[a-zA-Z\d*.@_]{8,28}$",
            ErrorMessage = "La contraseña debe incluir al menos 2 letras minúsculas, 2 letras mayúsculas, 2 números y 2 caracteres especiales permitidos (*, ., @, _)."
        )]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "La repetición de la contraseña es obligatoria para el cambio de contraseña.")]
        public string RepeatPassword { get; set; } = string.Empty;
    }
}