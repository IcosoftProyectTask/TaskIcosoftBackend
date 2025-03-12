using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskIcosoftBackend.Dtos.SupportTasks
{
    public class UpdateSupportTask
    {
        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(100, ErrorMessage = "El título no puede exceder los 100 caracteres.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [StringLength(50, ErrorMessage = "La categoría no puede exceder los 50 caracteres.")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "El ID del empleado a asignar obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del empleado a a asignar debe ser un número válido.")]
        public int IdUser { get; set; }

        [Required(ErrorMessage = "El ID de la empresa es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID de la empresa debe ser un número válido.")]
        public int IdCompany { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El ID del empleado de la empresa debe ser un número válido.")]
        public int IdCompanyEmployee { get; set; }

        [Required(ErrorMessage = "La prioridad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La prioridad debe ser un número válido.")]
        public int IdPriority { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El estado debe ser un número válido.")]
        public int IdStatus { get; set; } = 1;

        [StringLength(1000, ErrorMessage = "La solución no puede exceder los 1000 caracteres.")]
        public string? Solution { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "La fecha de inicio debe ser válida.")]
        public DateTime? StartTask { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "La fecha de finalización debe ser válida.")]
        public DateTime? EndTask { get; set; }
    }
}