using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskIcosoftBackend.Dtos.StatusTasks;
using TaskIcosoftBackend.Dtos.StatusTasksDto;
using TaskIcosoftBackend.Models;

namespace TaskIcosoftBackend.Mappers
{
    public static class StatusTaskMapper
    {
        public static StatusTaskDto statusTaskDto (this StatusTask statusTask)
        {
            return new StatusTaskDto
            {
                IdStatus = statusTask.IdStatus,
                Name = statusTask.Name,
                CreatedAt = statusTask.CreatedAt,
                UpdatedAt = statusTask.UpdatedAt,
                Status = statusTask.Status
            };
        }


        public static StatusTask statusTask (this CreateStatusTaskDto createStatusTask)
        {
            return new StatusTask
            {
                Name = createStatusTask.Name,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

        }
        
    }
}