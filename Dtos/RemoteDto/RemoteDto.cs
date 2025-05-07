using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskIcosoftBackend.Dtos.RemoteDto
{
    public class RemoteDto
    {
        public int IdRemote { get; set; }
        public string Customer { get; set; } = string.Empty;
        public string Terminal { get; set; } = string.Empty;
        public string Software { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Status { get; set; }
    }
}