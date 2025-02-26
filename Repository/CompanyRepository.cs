using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskIcosoftBackend.Data;
using TaskIcosoftBackend.Models;

namespace TaskIcosoftBackend.Repository
{
    public class CompanyRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<CompanyRepository> _logger;

        public CompanyRepository(DataContext context, ILogger<CompanyRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Company> CreateCompany(Company company)
        {
            try
            {
                await _context.Companys.AddAsync(company);
                await _context.SaveChangesAsync();
                return company;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al crear la compañía.");
                throw new ApplicationException("Error al crear la compañía.", e);
            }
        }

        public async Task<Company> GetCompanyById(int id)
        {
            try
            {
                return await _context.Companys
                    .Where(c => c.Status == true) // Solo compañías activas
                    .FirstOrDefaultAsync(c => c.IdCompany == id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error al obtener la compañía con ID {id}.");
                throw new ApplicationException($"Error al obtener la compañía con ID {id}.", e);
            }
        }

        public async Task<List<Company>> GetCompanys()
        {
            try
            {
                return await _context.Companys
                    .Where(c => c.Status == true) // Solo compañías activas
                    .ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al obtener la lista de compañías.");
                throw new ApplicationException("Error al obtener la lista de compañías.", e);
            }
        }

        public async Task<Company> UpdateCompany(Company company)
        {
            try
            {
                _context.Companys.Update(company);
                await _context.SaveChangesAsync();
                return company;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error al actualizar la compañía con ID {company.IdCompany}.");
                throw new ApplicationException($"Error al actualizar la compañía con ID {company.IdCompany}.", e);
            }
        }

        public async Task<bool> DeleteCompany(int id)
        {
            try
            {
                var company = await _context.Companys.FindAsync(id);
                if (company == null)
                {
                    _logger.LogWarning($"No se encontró la compañía con ID {id}.");
                    return false;
                }

                // Eliminación lógica: actualizar el Status a 0
                company.Status = false;
                _context.Companys.Update(company);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error al eliminar la compañía con ID {id}.");
                throw new ApplicationException($"Error al eliminar la compañía con ID {id}.", e);
            }
        }
    }
}