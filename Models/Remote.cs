using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskIcosoftBackend.Models
{
    public class Remote
    {
        [Key]
        public int IdRemote { get; set; }

        [Required]
        [MaxLength(100)]
        public string Customer { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Terminal { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Software { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string IpAddress { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string User { get; set; } = string.Empty;

        [Required]
        [MaxLength(256)]
        public string Password { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public bool Status { get; set; } // Estado activo/inactivo

    }
}