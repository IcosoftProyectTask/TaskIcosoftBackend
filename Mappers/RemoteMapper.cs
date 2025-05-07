using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskIcosoftBackend.Dtos.RemoteDto;
using TaskIcosoftBackend.Models;

namespace TaskIcosoftBackend.Mappers
{
    public static class RemoteMapper
    {
         public static RemoteDto ToDto(this Remote remote)
        {
            return new RemoteDto
            {
                IdRemote = remote.IdRemote,
                Customer = remote.Customer,
                Terminal = remote.Terminal,
                Software = remote.Software,
                IpAddress = remote.IpAddress,
                User = remote.User,
                Password = remote.Password,
                CreatedAt = remote.CreatedAt,
                UpdatedAt = remote.UpdatedAt,
                Status = remote.Status
            };
        }

        public static Remote ToModel(this CreateRemoteDto createRemoteDto)
        {
            return new Remote
            {
                Customer = createRemoteDto.Customer,
                Terminal = createRemoteDto.Terminal,
                Software = createRemoteDto.Software,
                IpAddress = createRemoteDto.IpAddress,
                User = createRemoteDto.User,
                Password = createRemoteDto.Password,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Status = true
            };
        }
        
    }
}