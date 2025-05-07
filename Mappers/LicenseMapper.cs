using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskIcosoftBackend.Dtos.LicenseDto;
using TaskIcosoftBackend.Models;

namespace TaskIcosoftBackend.Mappers
{
    public static class LicenseMapper
    {
        public static LicenseDto ToDto(this License license)
        {
            return new LicenseDto
            {
                IdLicense = license.IdLicense,
                Client = license.Client,
                DeviceName = license.DeviceName,
                LicenseNumber = license.LicenseNumber,
                Type = license.Type,
                InstallationDate = license.InstallationDate,
                CreatedAt = license.CreatedAt,
                UpdatedAt = license.UpdatedAt,
                Status = license.Status
            };
        }

        public static License ToModel(this CreateLicenseDto createLicenseDto)
        {
            return new License
            {
                Client = createLicenseDto.Client,
                DeviceName = createLicenseDto.DeviceName,
                LicenseNumber = createLicenseDto.LicenseNumber,
                Type = createLicenseDto.Type,
                InstallationDate = createLicenseDto.InstallationDate,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Status = true
            };
        }
    }
}