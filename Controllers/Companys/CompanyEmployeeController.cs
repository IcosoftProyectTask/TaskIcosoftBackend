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
    [Route("api/CompanyEmployee")]
    public class CompanyEmployeeController : ControllerBase
    {
        private readonly CompanyEmployeeService _companyEmployeeService;
        private readonly ILogger<CompanyEmployeeController> _logger;

        public CompanyEmployeeController(CompanyEmployeeService companyEmployeeService, ILogger<CompanyEmployeeController> logger)
        {
            _companyEmployeeService = companyEmployeeService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompanyEmployee([FromBody] CreateCompanyEmployeeDto companyEmployeeDto)
        {
            _logger.LogInformation("Creando un nuevo empleado de compañía.");
            try
            {
                var companyEmployee = companyEmployeeDto.ToModel();
                var result = await _companyEmployeeService.CreateCompanyEmployee(companyEmployee);
                _logger.LogInformation("Empleado de compañía creado exitosamente con ID {CompanyEmployeeId}.", result.IdCompanyEmployee);
                return Ok(ApiResponse<CompanyEmployeeDto>.Ok(result.ToDto(), "Empleado de compañía creado exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el empleado de compañía.");
                return StatusCode(500, ApiResponse<string>.Error("Error al crear el empleado de compañía."));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyEmployeeById(int id)
        {
            _logger.LogInformation("Obteniendo empleado de compañía con ID {CompanyEmployeeId}.", id);
            try
            {
                var employee = await _companyEmployeeService.GetCompanyEmployeeById(id);
                if (employee == null)
                {
                    _logger.LogWarning("No se encontró el empleado de compañía con ID {CompanyEmployeeId}.", id);
                    return NotFound(ApiResponse<string>.Error("No se encontró el empleado de compañía."));
                }
                return Ok(ApiResponse<CompanyEmployeeDto>.Ok(employee.ToDto(), "Empleado de compañía encontrado."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el empleado de compañía con ID {CompanyEmployeeId}.", id);
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener el empleado de compañía."));
            }
        }

        [HttpGet("company/{idCompany}")]
        public async Task<IActionResult> GetCompanyEmployeesByCompanyId(int idCompany)
        {
            _logger.LogInformation("Obteniendo empleados de la compañía con ID {CompanyId}.", idCompany);
            try
            {
                var employees = await _companyEmployeeService.GetCompanyEmployeeByIdCompany(idCompany);
                if (employees == null )
                {
                    _logger.LogWarning("No se encontraron empleados para la compañía con ID {CompanyId}.", idCompany);
                    return NotFound(ApiResponse<string>.Error("No se encontraron empleados para la compañía."));
                }
                return Ok(ApiResponse<CompanyEmployeeDto>.Ok(employees.ToDto(), "Empleado de compañía encontrado."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener empleados de la compañía con ID {CompanyId}.", idCompany);
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener los empleados de la compañía."));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCompanyEmployees()
        {
            _logger.LogInformation("Obteniendo lista de empleados de compañía.");
            try
            {
                var employees = await _companyEmployeeService.GetCompanyEmployees();
                return Ok(ApiResponse<IEnumerable<CompanyEmployeeDto>>.Ok(employees.Select(e => e.ToDto()), "Lista de empleados obtenida."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de empleados de compañía.");
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener la lista de empleados de compañía."));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompanyEmployee(int id, [FromBody] UpdateCompanyEmployeeDto companyEmployeeDto)
        {
            _logger.LogInformation("Actualizando empleado de compañía con ID {CompanyEmployeeId}.", id);
            try
            {
                var existingEmployee = await _companyEmployeeService.GetCompanyEmployeeById(id);
                if (existingEmployee == null)
                {
                    _logger.LogWarning("No se encontró el empleado de compañía con ID {CompanyEmployeeId}.", id);
                    return NotFound(ApiResponse<string>.Error("No se encontró el empleado de compañía."));
                }

                existingEmployee.NameEmployee = companyEmployeeDto.NameEmployee;
                existingEmployee.FirstSurname = companyEmployeeDto.FirstSurname;
                existingEmployee.SecondSurname = companyEmployeeDto.SecondSurname;
                existingEmployee.IdCompany = companyEmployeeDto.IdCompany;
                existingEmployee.UpdatedAt = DateTime.UtcNow;

                var updatedEmployee = await _companyEmployeeService.UpdateCompanyEmployee(existingEmployee);
                _logger.LogInformation("Empleado de compañía con ID {CompanyEmployeeId} actualizado exitosamente.", id);
                return Ok(ApiResponse<CompanyEmployeeDto>.Ok(updatedEmployee.ToDto(), "Empleado de compañía actualizado exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el empleado de compañía con ID {CompanyEmployeeId}.", id);
                return StatusCode(500, ApiResponse<string>.Error("Error al actualizar el empleado de compañía."));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompanyEmployee(int id)
        {
            _logger.LogInformation("Eliminando empleado de compañía con ID {CompanyEmployeeId}.", id);
            try
            {
                var result = await _companyEmployeeService.DeleteCompanyEmployee(id);
                if (result)
                {
                    _logger.LogInformation("Empleado de compañía con ID {CompanyEmployeeId} eliminado exitosamente.", id);
                    return Ok(ApiResponse<string>.Ok(null, "Empleado de compañía eliminado exitosamente."));
                }
                else
                {
                    _logger.LogWarning("No se encontró el empleado de compañía con ID {CompanyEmployeeId}.", id);
                    return NotFound(ApiResponse<string>.Error("No se encontró el empleado de compañía."));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el empleado de compañía con ID {CompanyEmployeeId}.", id);
                return StatusCode(500, ApiResponse<string>.Error("Error al eliminar el empleado de compañía."));
            }
        }
    }
}
