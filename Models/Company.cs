using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskIcosoftBackend.Models
{
    public class Company
    {
        [Key]
        public int IdCompany { get; set; }

        [Required]
        [MaxLength(100)]
        public string CompanyName { get; set; } = string.Empty; 

        [Required]
        [MaxLength(100)]
        public string CompanyAddress { get; set; } = string.Empty;

        [Required]
        [MaxLength(40)]
        public string Province { get; set; } = string.Empty;

        [Required]
        [MaxLength(40)]
        public string Canton { get; set; } = string.Empty;

        [Required]
        [MaxLength(40)]
        public string Distric { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string CompanyPhone { get; set; } = string.Empty;

        [Required]

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public bool Status { get; set; }


    
        
    }
}