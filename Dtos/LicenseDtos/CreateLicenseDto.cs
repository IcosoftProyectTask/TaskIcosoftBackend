using System;
using System.ComponentModel.DataAnnotations;

namespace TaskIcosoftBackend.Dtos.LicenseDto
{
    public class CreateLicenseDto
    {
      
        public string Client { get; set; } = string.Empty;
        public string DeviceName { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime InstallationDate { get; set; }
       // public string VendorAccount { get; set; } = string.Empty;
    }
}