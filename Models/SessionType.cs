using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace gymconnect_backend.Models
{
    public class SessionType
    {
        [Key]
        public int IdSessionType { get; set; }

        [Required]
        [MaxLength(100)]
        public string SessionTypeName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public bool Status { get; set; }
    }
}