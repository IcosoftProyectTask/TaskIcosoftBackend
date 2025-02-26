using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace TaskIcosoftBackend.Models
{
    public class CompanyEmployees
    {
        [Key]  
        public int IdCompanyEmployee { get; set; }

        [Required]
        [MaxLength(50)]
        public string NameEmployee {get; set;} = string.Empty;

        [Required]
        [MaxLength(50)]
        public string FirstSurname { get; set; } = string.Empty; 

        [Required]
        [MaxLength(50)]
        public string SecondSurname { get; set; } = string.Empty;

        [Required]
        public int IdCompany { get; set; }

        [ForeignKey("IdCompany")]
        public Company? Company { get; set; }

        [Required]

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public bool Status { get; set; }
    }
}