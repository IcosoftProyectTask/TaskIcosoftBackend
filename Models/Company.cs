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
        public string CompanyFiscalName { get; set; } = string.Empty; 

        [Required]
        [MaxLength(100)]
        public string CompanyComercialName { get; set; } = string.Empty; 

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty; // Email

        [Required]
        [MaxLength(100)]
        public string CompanyAddress { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(15)]
        public string IdCart { get; set; } = string.Empty;

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