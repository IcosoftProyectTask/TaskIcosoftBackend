using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaskIcosoftBackend.Models
{
    public class SupportTasks
    {
        [Key]
        public int IdSupportTask { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Category { get; set; } = string.Empty;

        [Required]
        public int IdUser { get; set; }

        [ForeignKey("IdUser")]
        public User? User { get; set; }

        [Required]
        public int IdCompany { get; set; }

        [ForeignKey("IdCompany")]
        public Company? Company { get; set; }

        [Required]
        [MaxLength(100)]
        public string NameEmployeeCompany { get; set; } = string.Empty;

        public string? Solution { get; set; } // Descripción de la solución aplicada

        [Required]
        public int IdPriority { get; set; } // Prioridad de la tarea

        [ForeignKey("IdPriority")]
        public Priority? Priority { get; set; }

        [Required]
        public int IdStatus { get; set; } // Estado de la tarea

        [ForeignKey("IdStatus")]
        public StatusTask? StatusTask { get; set; }

        [Required]
        public DateTime? StartTask { get; set; } // Fecha de resolución

        [Required]
        public DateTime? EndTask{ get; set; } 

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        [Required]
        public bool Status { get; set; }  = true;      
    }
}