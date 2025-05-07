using System;

namespace TaskIcosoftBackend.Dtos.LicenseDto
{
    public class LicenseDto
    {
        public int IdLicense { get; set; }
        public string Client { get; set; } = string.Empty;
        public string DeviceName { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime InstallationDate { get; set; }
    //    public string VendorAccount { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Status { get; set; }
    }
}