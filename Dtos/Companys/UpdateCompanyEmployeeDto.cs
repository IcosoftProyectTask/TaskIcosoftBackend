using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskIcosoftBackend.Dtos.Companys
{
    public class UpdateCompanyEmployeeDto
    {
        [Required(ErrorMessage = "El nombre del empleado es obligatorio.")]
        [MinLength(3, ErrorMessage = "El nombre del empleado debe tener al menos 2 caracteres.")]
        [MaxLength(50, ErrorMessage = "El nombre del empleado debe tener como máximo 50 caracteres.")]
        public string NameEmployee { get; set; } = string.Empty;

        [Required(ErrorMessage = "El primer apellido del empleado es obligatorio.")]
        [MinLength(3, ErrorMessage = "El primer apellido del empleado debe tener al menos 3 caracteres.")]
        [MaxLength(50, ErrorMessage = "El primer apellido del empleado debe tener como máximo 20 caracteres.")]
        public string FirstSurname { get; set; } = string.Empty;

        [Required(ErrorMessage = "El segundo apellido del empleado es obligatorio.")]
        [MinLength(3, ErrorMessage = "El segundo apellido del empleado debe tener al menos 3 caracteres.")]
        [MaxLength(50, ErrorMessage = "El segundo apellido del empleado debe tener como máximo 20 caracteres.")]
        public string SecondSurname { get; set; } = string.Empty;

        [Required(ErrorMessage = "El id de la empresa es obligatorio.")]
        public int IdCompany { get; set; }
    }
}