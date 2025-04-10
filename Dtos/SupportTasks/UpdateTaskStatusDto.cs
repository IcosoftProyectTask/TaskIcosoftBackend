using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskIcosoftBackend.Dtos.SupportTasks
{
    public class UpdateTaskStatusDto
    {
        public int IdStatus { get; set; }
        public DateTime? EndTask { get; set; }
    }
}