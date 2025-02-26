using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskIcosoftBackend.Common;
using TaskIcosoftBackend.Dtos.Companys;
using TaskIcosoftBackend.Mappers;
using TaskIcosoftBackend.Models;
using TaskIcosoftBackend.Service;

namespace TaskIcosoftBackend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/Company")]
    public class CompanyController : ControllerBase
    {
        private readonly CompanyService _companyService;
        private readonly ILogger<CompanyController> _logger;

        public CompanyController(CompanyService companyService, ILogger<CompanyController> logger)
        {
            _companyService = companyService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyDto companyDto)
        {
            _logger.LogInformation("Creando una nueva compañía.");
            try
            {
                var company = companyDto.ToModel();
                var result = await _companyService.CreateCompany(company);
                _logger.LogInformation("Compañía creada exitosamente con ID {CompanyId}.", result.IdCompany);
                return Ok(ApiResponse<CompanyDto>.Ok(result.ToDto(), "Compañía creada exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la compañía.");
                return StatusCode(500, ApiResponse<string>.Error("Error al crear la compañía."));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyById(int id)
        {
            _logger.LogInformation("Obteniendo compañía con ID {CompanyId}.", id);
            try
            {
                var company = await _companyService.GetCompanyById(id);
                if (company == null)
                {
                    _logger.LogWarning("No se encontró la compañía con ID {CompanyId}.", id);
                    return NotFound(ApiResponse<string>.Error("No se encontró la compañía."));
                }
                return Ok(ApiResponse<CompanyDto>.Ok(company.ToDto(), "Compañía encontrada."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la compañía con ID {CompanyId}.", id);
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener la compañía."));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            _logger.LogInformation("Obteniendo lista de compañías.");
            try
            {
                var companies = await _companyService.GetCompanys();
                return Ok(ApiResponse<IEnumerable<CompanyDto>>.Ok(companies.Select(c => c.ToDto()), "Lista de compañías obtenida."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de compañías.");
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener la lista de compañías."));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, [FromBody] UpdateCompanyDto companyDto)
        {
            _logger.LogInformation("Actualizando compañía con ID {CompanyId}.", id);
            try
            {
                var existingCompany = await _companyService.GetCompanyById(id);
                if (existingCompany == null)
                {
                    _logger.LogWarning("No se encontró la compañía con ID {CompanyId}.", id);
                    return NotFound(ApiResponse<string>.Error("No se encontró la compañía."));
                }

                existingCompany.CompanyFiscalName = companyDto.CompanyFiscalName;
                existingCompany.CompanyComercialName = companyDto.CompanyComercialName;
                existingCompany.Email = companyDto.Email;
                existingCompany.CompanyAddress = companyDto.CompanyAddress;
                existingCompany.IdCart = companyDto.IdCart;
                existingCompany.UpdatedAt = DateTime.UtcNow;

                var updatedCompany = await _companyService.UpdateCompany(existingCompany);
                _logger.LogInformation("Compañía con ID {CompanyId} actualizada exitosamente.", id);
                return Ok(ApiResponse<CompanyDto>.Ok(updatedCompany.ToDto(), "Compañía actualizada exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la compañía con ID {CompanyId}.", id);
                return StatusCode(500, ApiResponse<string>.Error("Error al actualizar la compañía."));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            _logger.LogInformation("Eliminando compañía con ID {CompanyId}.", id);
            try
            {
                var result = await _companyService.DeleteCompany(id);
                if (result)
                {
                    _logger.LogInformation("Compañía con ID {CompanyId} eliminada exitosamente.", id);
                    return Ok(ApiResponse<string>.Ok(null, "Compañía eliminada exitosamente."));
                }
                else
                {
                    _logger.LogWarning("No se encontró la compañía con ID {CompanyId}.", id);
                    return NotFound(ApiResponse<string>.Error("No se encontró la compañía."));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la compañía con ID {CompanyId}.", id);
                return StatusCode(500, ApiResponse<string>.Error("Error al eliminar la compañía."));
            }
        }
    }
}
