using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskIcosoftBackend.Dtos.SessionType
{
    public class SessionType
    {
        public int IdSessionType { get; set; }
        public string SessionTypeName { get; set; } = string.Empty;
        public bool Status { get; set; }
    }
}