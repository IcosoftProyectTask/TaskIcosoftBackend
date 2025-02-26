using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gymconnect_backend.Models;
using TaskIcosoftBackend.Dtos.Role;

namespace TaskIcosoftBackend.Mappers
{
    public static class RoleMapper
    {
        public static RoleDto? ToDto(Role? role)
        {
            if (role == null)
                return null;

            return new RoleDto
            {
                IdRole = role.IdRole,
                RoleName = role.RoleName,
                CreatedAt = role.CreatedAt,
                UpdatedAt = role.UpdatedAt,
                Status = role.Status
            };
        }

        public static Role? ToModel(RoleDto? roleDto)
        {
            if (roleDto == null)
                return null;

            return new Role
            {
                IdRole = roleDto.IdRole,
                RoleName = roleDto.RoleName,
                CreatedAt = roleDto.CreatedAt,
                UpdatedAt = roleDto.UpdatedAt,
                Status = roleDto.Status
            };
        }

    }
}