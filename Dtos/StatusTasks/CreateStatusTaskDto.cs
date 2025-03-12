using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskIcosoftBackend.Dtos.StatusTasks
{
    public class CreateStatusTaskDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MinLength(3, ErrorMessage = "El nombre debe tener al menos 3 caracteres.")]
        [MaxLength(12, ErrorMessage = "El nombre debe tener como máximo 12 caracteres.")]
        public string Name { get; set; }
    }
}