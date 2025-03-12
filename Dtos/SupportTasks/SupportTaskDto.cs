using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskIcosoftBackend.Dtos.Companys;
using TaskIcosoftBackend.Dtos.PriorityDtos;
using TaskIcosoftBackend.Dtos.StatusTasksDto;
using TaskIcosoftBackend.Dtos.User;

namespace TaskIcosoftBackend.Dtos.SupportTasks
{
    public class SupportTaskDto
    {
        public int IdSupportTask { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int IdUser { get; set; }
        public UserDto? User { get; set; }
        public int IdCompany { get; set; }
        public CompanyDto? Company { get; set; }
        public int IdCompanyEmployee { get; set; }
        public CompanyEmployeeDto? CompanyEmployees { get; set; }
        public int IdPriority { get; set; }
        public PriorityDto? Priority { get; set; }
        public int IdStatus { get; set; }
        public StatusTaskDto? StatusTask { get; set; }
        public string? Solution { get; set; }
        public DateTime? StartTask { get; set; }
        public DateTime? EndTask { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}