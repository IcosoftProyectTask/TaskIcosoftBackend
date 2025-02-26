using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gymconnect_backend.Models;
using TaskIcosoftBackend.Dtos.SessionTypeDto;

namespace TaskIcosoftBackend.Mappers
{
    public static class SessionTypeMapper
    {
     public static SessionTypeDto ToDto(SessionType sessionType)
        {
            return new SessionTypeDto
            {
                IdSessionType = sessionType.IdSessionType,
                SessionTypeName = sessionType.SessionTypeName,
                Status = sessionType.Status
            };
        }

        public static SessionType ToModel(SessionTypeDto dto)
        {
            return new SessionType
            {
                IdSessionType = dto.IdSessionType,
                SessionTypeName = dto.SessionTypeName,
                Status = dto.Status
            };
        }   
    }
}