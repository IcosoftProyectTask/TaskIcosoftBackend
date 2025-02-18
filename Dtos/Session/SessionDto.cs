using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gymconnect_backend.Dtos.Session
{
    public class SessionDto
    {
        public int IdSession { get; set; }
        public int IdUser { get; set; }
        public int IdSessionType { get; set; }
        [Required]
        [MaxLength(500)]
        public string SessionToken { get; set; } = string.Empty;
        public string? IpAddress { get; set; } = string.Empty;
        public string DeviceInfo { get; set; } = string.Empty;
        public DateTime ExpirationSessionDate { get; set; }
        public DateTime LastActivityDate { get; set; }
        public bool Status { get; set; }
    }
}