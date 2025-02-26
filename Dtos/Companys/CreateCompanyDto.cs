using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskIcosoftBackend.Dtos.Companys
{
    public class CreateCompanyDto
    {
        [Required(ErrorMessage = "El nombre fiscal de la empresa es obligatorio.")]
        [MinLength(3, ErrorMessage = "El nombre fiscal debe tener al menos 3 caracteres.")]
        [MaxLength(50, ErrorMessage = "El nombre fiscal debe tener como máximo 50 caracteres.")]
        public string CompanyFiscalName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre comercial de la empresa es obligatorio.")]
        [MinLength(3, ErrorMessage = "El nombre comercial debe tener al menos 3 caracteres.")]
        [MaxLength(50, ErrorMessage = "El nombre comercial debe tener como máximo 50 caracteres.")]
        public string CompanyComercialName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección de la empresa es obligatoria.")]
        [MinLength(5, ErrorMessage = "La dirección debe tener al menos 5 caracteres.")]
        [MaxLength(100, ErrorMessage = "La dirección debe tener como máximo 100 caracteres.")]
        public string CompanyAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "El número de identificación es obligatorio.")]
        [RegularExpression(@"^\d{9,13}$", ErrorMessage = "El número de identificación debe tener entre 9 y 10 dígitos.")]
        public string IdCart { get; set; } = string.Empty;

        [Required(ErrorMessage = "El número de teléfono es obligatorio.")]
        [RegularExpression(@"^\d{8,15}$", ErrorMessage = "El número de teléfono debe tener entre 8 y 15 dígitos.")]
        public string CompanyPhone { get; set; } = string.Empty;
    }
}