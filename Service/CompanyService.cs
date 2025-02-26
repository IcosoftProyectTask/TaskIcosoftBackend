using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TaskIcosoftBackend.Models;
using TaskIcosoftBackend.Repository;

namespace TaskIcosoftBackend.Service
{
    public class CompanyService
    {
        private readonly CompanyRepository _companyRepository;
        private readonly ILogger<CompanyService> _logger;

        public CompanyService(CompanyRepository companyRepository, ILogger<CompanyService> logger)
        {
            _companyRepository = companyRepository;
            _logger = logger;
        }

        public async Task<Company> CreateCompany(Company company)
        {
            _logger.LogInformation("Creando una nueva compañía.");
            try
            {
                var result = await _companyRepository.CreateCompany(company);
                _logger.LogInformation("Compañía creada exitosamente con ID {CompanyId}.", result.IdCompany);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la compañía.");
                throw; // Relanzar la excepción para que la capa superior la maneje
            }
        }

        public async Task<Company> GetCompanyById(int id)
        {
            _logger.LogInformation("Obteniendo compañía con ID {CompanyId}.", id);
            try
            {
                var company = await _companyRepository.GetCompanyById(id);
                if (company == null)
                {
                    _logger.LogWarning("No se encontró la compañía con ID {CompanyId}.", id);
                }
                else
                {
                    _logger.LogInformation("Compañía con ID {CompanyId} obtenida exitosamente.", id);
                }
                return company;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la compañía con ID {CompanyId}.", id);
                throw;
            }
        }

        public async Task<List<Company>> GetCompanys()
        {
            _logger.LogInformation("Obteniendo lista de compañías.");
            try
            {
                var companies = await _companyRepository.GetCompanys();
                _logger.LogInformation("Se obtuvieron {Count} compañías exitosamente.", companies.Count);
                return companies;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de compañías.");
                throw;
            }
        }

        public async Task<Company> UpdateCompany(Company company)
        {
            _logger.LogInformation("Actualizando compañía con ID {CompanyId}.", company.IdCompany);
            try
            {
                var result = await _companyRepository.UpdateCompany(company);
                _logger.LogInformation("Compañía con ID {CompanyId} actualizada exitosamente.", company.IdCompany);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la compañía con ID {CompanyId}.", company.IdCompany);
                throw;
            }
        }

        public async Task<bool> DeleteCompany(int id)
        {
            _logger.LogInformation("Eliminando compañía con ID {CompanyId}.", id);
            try
            {
                var result = await _companyRepository.DeleteCompany(id);
                if (result)
                {
                    _logger.LogInformation("Compañía con ID {CompanyId} eliminada exitosamente.", id);
                }
                else
                {
                    _logger.LogWarning("No se pudo eliminar la compañía con ID {CompanyId}.", id);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la compañía con ID {CompanyId}.", id);
                throw;
            }
        }
    }
}