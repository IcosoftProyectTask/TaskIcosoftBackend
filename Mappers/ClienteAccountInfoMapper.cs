using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskIcosoftBackend.Dtos.ClienteAccountInfoDto;
using TaskIcosoftBackend.Models;

namespace TaskIcosoftBackend.Mappers
{
    public static class ClienteAccountInfoMapper
    {
        public static ClienteAccountInfoDto ToDto(this ClienteAccountInfo clienteAccountInfo)
        {
            return new ClienteAccountInfoDto
            {
                IdClienteAccountInfo = clienteAccountInfo.IdClienteAccountInfo,
                Client = clienteAccountInfo.Client,
                Email = clienteAccountInfo.Email,
                Password = clienteAccountInfo.Password,
                AppPassword = clienteAccountInfo.AppPassword,
                Vin = clienteAccountInfo.Vin,
                Date1 = clienteAccountInfo.Date1,
                CreatedAt = clienteAccountInfo.CreatedAt,
                UpdatedAt = clienteAccountInfo.UpdatedAt,
                Status = clienteAccountInfo.Status
            };
        }

        public static ClienteAccountInfo ToModel(this CreateClienteAccountInfoDto createClienteAccountInfoDto)
        {
            return new ClienteAccountInfo
            {
                Client = createClienteAccountInfoDto.Client,
                Email = createClienteAccountInfoDto.Email,
                Password = createClienteAccountInfoDto.Password,
                AppPassword = createClienteAccountInfoDto.AppPassword,
                Vin = createClienteAccountInfoDto.Vin,
                Date1 = createClienteAccountInfoDto.Date1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Status = true
            };
        }
    }
}