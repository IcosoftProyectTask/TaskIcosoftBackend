using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskIcosoftBackend.Dtos.PriorityDtos;
using TaskIcosoftBackend.Models;

namespace TaskIcosoftBackend.Mappers
{
    public static class PriorityMapper
    {
     public static PriorityDto ToDto(this Priority priority)
     {
         return new PriorityDto
         {
             IdPriority = priority.IdPriority,
             Name = priority.Name,
             CreatedAt = priority.CreatedAt,
             UpdatedAt = priority.UpdatedAt,
             Status = priority.Status
         };
     }

        public static Priority ToModel(this CreatePriorityDto createPriority)
        {
            return new Priority
            {
                Name = createPriority.Name,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

        }   
    }
}