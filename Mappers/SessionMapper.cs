using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gymconnect_backend.Dtos.Session;
using gymconnect_backend.Models;

namespace gymconnect_backend.Mappers
{
public static class SessionMapper
    {
        public static SessionDto ToDto(Session session)
        {
            return new SessionDto
            {
                IdSession = session.IdSession,
                IdUser = session.IdUser,
                IdSessionType = session.IdSessionType,
                SessionToken = session.SessionToken,
                IpAddress = session.IpAddress,
                DeviceInfo = session.DeviceInfo,
                ExpirationSessionDate = session.ExpirationSessionDate,
                LastActivityDate = session.LastActivityDate,
                Status = session.Status
            };
        }

        public static Session ToModel(SessionDto dto)
        {
            return new Session
            {
                IdSession = dto.IdSession,
                IdUser = dto.IdUser,
                IdSessionType = dto.IdSessionType,
                SessionToken = dto.SessionToken,
                IpAddress = dto.IpAddress,
                DeviceInfo = dto.DeviceInfo,
                ExpirationSessionDate = dto.ExpirationSessionDate,
                LastActivityDate = dto.LastActivityDate,
                Status = dto.Status
            };
        }
    }
}