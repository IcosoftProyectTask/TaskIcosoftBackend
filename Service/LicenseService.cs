using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TaskIcosoftBackend.Models;
using TaskIcosoftBackend.Repository;

namespace TaskIcosoftBackend.Service
{
    public class LicenseService
    {
        private readonly LicenseRepository _licenseRepository;
        private readonly ILogger<LicenseService> _logger;

        public LicenseService(LicenseRepository licenseRepository, ILogger<LicenseService> logger)
        {
            _licenseRepository = licenseRepository;
            _logger = logger;
        }

        public async Task<License> CreateLicense(License license)
        {
            _logger.LogInformation("Creando una nueva licencia.");
            try
            {
                // Nota: La encriptación se maneja en el repositorio
                var result = await _licenseRepository.CreateLicense(license);
                _logger.LogInformation("Licencia creada exitosamente con ID {LicenseId}.", result.IdLicense);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la licencia.");
                throw; // Relanzar la excepción para que la capa superior la maneje
            }
        }

        public async Task<License> GetLicenseById(int id)
        {
            _logger.LogInformation("Obteniendo licencia con ID {LicenseId}.", id);
            try
            {
                var license = await _licenseRepository.GetLicenseById(id);
                if (license == null)
                {
                    _logger.LogWarning("No se encontró la licencia con ID {LicenseId}.", id);
                }
                else
                {
                    _logger.LogInformation("Licencia con ID {LicenseId} obtenida exitosamente.", id);
                }
                return license;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la licencia con ID {LicenseId}.", id);
                throw;
            }
        }

        public async Task<List<License>> GetLicenses()
        {
            _logger.LogInformation("Obteniendo lista de licencias.");
            try
            {
                var licenses = await _licenseRepository.GetLicenses();
                _logger.LogInformation("Se obtuvieron {Count} licencias exitosamente.", licenses.Count);
                return licenses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de licencias.");
                throw;
            }
        }

        public async Task<License> UpdateLicense(License license)
        {
            _logger.LogInformation("Actualizando licencia con ID {LicenseId}.", license.IdLicense);
            try
            {
                // Nota: La verificación y encriptación se maneja en el repositorio
                var result = await _licenseRepository.UpdateLicense(license);
                _logger.LogInformation("Licencia con ID {LicenseId} actualizada exitosamente.", license.IdLicense);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la licencia con ID {LicenseId}.", license.IdLicense);
                throw;
            }
        }

        public async Task<bool> DeleteLicense(int id)
        {
            _logger.LogInformation("Eliminando licencia con ID {LicenseId}.", id);
            try
            {
                var result = await _licenseRepository.DeleteLicense(id);
                if (result)
                {
                    _logger.LogInformation("Licencia con ID {LicenseId} eliminada exitosamente.", id);
                }
                else
                {
                    _logger.LogWarning("No se pudo eliminar la licencia con ID {LicenseId}.", id);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la licencia con ID {LicenseId}.", id);
                throw;
            }
        }
    }
}