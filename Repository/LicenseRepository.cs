using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskIcosoftBackend.Custom;
using TaskIcosoftBackend.Data;
using TaskIcosoftBackend.Models;

namespace TaskIcosoftBackend.Repository
{
    public class LicenseRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<LicenseRepository> _logger;

        public LicenseRepository(DataContext context, ILogger<LicenseRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<License> CreateLicense(License license)
        {
            try
            { 
                await _context.Licenses.AddAsync(license);
                await _context.SaveChangesAsync();
                return license;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al crear la licencia.");
                throw new ApplicationException("Error al crear la licencia.", e);
            }
        }

        public async Task<License> GetLicenseById(int id)
        {
            try
            {
                return await _context.Licenses
                    .Where(l => l.Status == true) // Solo licencias activas
                    .FirstOrDefaultAsync(l => l.IdLicense == id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error al obtener la licencia con ID {id}.");
                throw new ApplicationException($"Error al obtener la licencia con ID {id}.", e);
            }
        }

        public async Task<List<License>> GetLicenses()
        {
            try
            {
                return await _context.Licenses
                    .Where(l => l.Status == true) // Solo licencias activas
                    .ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al obtener la lista de licencias.");
                throw new ApplicationException("Error al obtener la lista de licencias.", e);
            }
        }

        public async Task<License> UpdateLicense(License license)
        {
            try
            {
                _context.Licenses.Update(license);
                await _context.SaveChangesAsync();
                return license;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error al actualizar la licencia con ID {license.IdLicense}.");
                throw new ApplicationException($"Error al actualizar la licencia con ID {license.IdLicense}.", e);
            }
        }

        public async Task<bool> DeleteLicense(int id)
        {
            try
            {
                var license = await _context.Licenses.FindAsync(id);
                if (license == null)
                {
                    _logger.LogWarning($"No se encontró la licencia con ID {id}.");
                    return false;
                }
                // Eliminación lógica: actualizar el Status a false
                license.Status = false;
                _context.Licenses.Update(license);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error al eliminar la licencia con ID {id}.");
                throw new ApplicationException($"Error al eliminar la licencia con ID {id}.", e);
            }
        }
    }
}