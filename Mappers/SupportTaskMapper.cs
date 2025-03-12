using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskIcosoftBackend.Dtos.PriorityDtos;
using TaskIcosoftBackend.Dtos.SupportTasks;
using TaskIcosoftBackend.Models;

namespace TaskIcosoftBackend.Mappers
{
    public static class SupportTaskMapper
    {
        public static SupportTaskDto ToDto(this SupportTasks supportTask)
        {
            return new SupportTaskDto
            {
                IdSupportTask = supportTask.IdSupportTask,
                Title = supportTask.Title,
                Description = supportTask.Description,
                Category = supportTask.Category,
                IdUser = supportTask.IdUser,
                User = supportTask.User != null ? UserMapper.ToDto(supportTask.User) : null,
                IdCompany = supportTask.IdCompany,
                Company = supportTask.Company != null ? CompanyMapper.ToDto(supportTask.Company) : null,
                IdCompanyEmployee = supportTask.IdCompanyEmployee,
                CompanyEmployees = supportTask.CompanyEmployees != null ? CompanyEmployeeMapper.ToDto(supportTask.CompanyEmployees) : null,
                IdPriority = supportTask.IdPriority,
                Priority = supportTask.Priority != null ? PriorityMapper.ToDto(supportTask.Priority) : null,
                IdStatus = supportTask.IdStatus,
                StatusTask = supportTask.StatusTask != null ? StatusTaskMapper.statusTaskDto(supportTask.StatusTask) : null,
                Solution = supportTask.Solution,
                StartTask = supportTask.StartTask,
                EndTask = supportTask.EndTask,
                CreatedAt = supportTask.CreatedAt,
                UpdatedAt = supportTask.UpdatedAt
                
            };
        }


        public static SupportTasks ToModel(this CreateSupportTask createSupportTask)
        {
            return new SupportTasks
            {
                Title = createSupportTask.Title,
                Description = createSupportTask.Description,
                Category = createSupportTask.Category,
                IdUser = createSupportTask.IdUser,
                IdCompany = createSupportTask.IdCompany,
                IdCompanyEmployee = createSupportTask.IdCompanyEmployee,
                IdPriority = createSupportTask.IdPriority,
                IdStatus = createSupportTask.IdStatus,
                Solution = createSupportTask.Solution,
                StartTask = createSupportTask.StartTask,
                EndTask = createSupportTask.EndTask,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        public static void UpdateStatus(this SupportTasks supportTask, UpdateStatusSupportTask dto)
        {
            supportTask.IdStatus = dto.IdStatus;
            supportTask.UpdatedAt = DateTime.UtcNow;
        }

        public static void UpdateUserAsigment(this SupportTasks supportTask, UpdateUserAsigmentDto dto)
        {
            supportTask.IdUser = dto.IdUser;
            supportTask.UpdatedAt = DateTime.UtcNow;
        }


    }
}