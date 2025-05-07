using System;
using System.ComponentModel.DataAnnotations;

namespace TaskIcosoftBackend.Dtos.LicenseDto
{
    public class UpdateLicenseDto
    {
        [Required(ErrorMessage = "El nombre del cliente es obligatorio.")]
        [MinLength(3, ErrorMessage = "El nombre del cliente debe tener al menos 3 caracteres.")]
        [MaxLength(100, ErrorMessage = "El nombre del cliente debe tener como máximo 100 caracteres.")]
        public string Client { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre del dispositivo es obligatorio.")]
        [MinLength(3, ErrorMessage = "El nombre del dispositivo debe tener al menos 3 caracteres.")]
        [MaxLength(100, ErrorMessage = "El nombre del dispositivo debe tener como máximo 100 caracteres.")]
        public string DeviceName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El número de licencia es obligatorio.")]
        [MinLength(5, ErrorMessage = "El número de licencia debe tener al menos 5 caracteres.")]
        [MaxLength(100, ErrorMessage = "El número de licencia debe tener como máximo 100 caracteres.")]
        public string LicenseNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de licencia es obligatorio.")]
        [MinLength(2, ErrorMessage = "El tipo de licencia debe tener al menos 2 caracteres.")]
        [MaxLength(50, ErrorMessage = "El tipo de licencia debe tener como máximo 50 caracteres.")]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de instalación es obligatoria.")]
        public DateTime InstallationDate { get; set; }
        /*
        [Required(ErrorMessage = "La cuenta del proveedor es obligatoria.")]
        [MinLength(3, ErrorMessage = "La cuenta del proveedor debe tener al menos 3 caracteres.")]
        [MaxLength(100, ErrorMessage = "La cuenta del proveedor debe tener como máximo 100 caracteres.")]
        public string VendorAccount { get; set; } = string.Empty;
        */
    }
}