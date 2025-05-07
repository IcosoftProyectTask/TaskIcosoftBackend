using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskIcosoftBackend.Models
{
    public class License
    {
        [Key]
        public int IdLicense { get; set; }

        [Required]
        [MaxLength(100)]
        public string Client { get; set; }

        [Required]
        [MaxLength(100)]
        public string DeviceName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LicenseNumber { get; set; }

        [Required]
        [MaxLength(50)]
        public string Type { get; set; }
        public DateTime InstallationDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public bool Status { get; set; } 

    }
}