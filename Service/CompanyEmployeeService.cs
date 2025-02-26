using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TaskIcosoftBackend.Models;
using TaskIcosoftBackend.Repository;

namespace TaskIcosoftBackend.Service
{
    public class CompanyEmployeeService
    {
        private readonly CompanyEmployeeRepository _companyEmployeeRepository;
        private readonly ILogger<CompanyEmployeeService> _logger;

        public CompanyEmployeeService(CompanyEmployeeRepository companyEmployeeRepository, ILogger<CompanyEmployeeService> logger)
        {
            _companyEmployeeRepository = companyEmployeeRepository;
            _logger = logger;
        }

        public async Task<CompanyEmployees> CreateCompanyEmployee(CompanyEmployees companyEmployee)
        {
            _logger.LogInformation("Creando un nuevo empleado de compañía.");
            try
            {
                var result = await _companyEmployeeRepository.CreateCompanyEmployee(companyEmployee);
                _logger.LogInformation("Empleado de compañía creado exitosamente con ID {CompanyEmployeeId}.", result.IdCompanyEmployee);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el empleado de la compañía.");
                throw;
            }
        }

        public async Task<CompanyEmployees> GetCompanyEmployeeById(int id)
        {
            _logger.LogInformation("Obteniendo empleado de compañía con ID {CompanyEmployeeId}.", id);
            try
            {
                var employee = await _companyEmployeeRepository.GetCompanyEmployeeById(id);
                if (employee == null)
                {
                    _logger.LogWarning("No se encontró el empleado de compañía con ID {CompanyEmployeeId}.", id);
                }
                else
                {
                    _logger.LogInformation("Empleado de compañía con ID {CompanyEmployeeId} obtenido exitosamente.", id);
                }
                return employee;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el empleado de la compañía con ID {CompanyEmployeeId}.", id);
                throw;
            }
        }

        public async Task<CompanyEmployees> GetCompanyEmployeeByIdCompany(int id)
        {
            _logger.LogInformation("Obteniendo empleado de compañía con ID de compañía {CompanyId}.", id);
            try
            {
                var employee = await _companyEmployeeRepository.GetCompanyEmployeeByIdCompany(id);
                if (employee == null)
                {
                    _logger.LogWarning("No se encontró empleado para la compañía con ID {CompanyId}.", id);
                }
                else
                {
                    _logger.LogInformation("Empleado para la compañía con ID {CompanyId} obtenido exitosamente.", id);
                }
                return employee;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el empleado de la compañía con ID {CompanyId}.", id);
                throw;
            }
        }

        public async Task<List<CompanyEmployees>> GetCompanyEmployees()
        {
            _logger.LogInformation("Obteniendo lista de empleados de la compañía.");
            try
            {
                var employees = await _companyEmployeeRepository.GetCompanyEmployees();
                _logger.LogInformation("Se obtuvieron {Count} empleados de compañía exitosamente.", employees.Count);
                return employees;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de empleados de la compañía.");
                throw;
            }
        }

        public async Task<CompanyEmployees> UpdateCompanyEmployee(CompanyEmployees companyEmployee)
        {
            _logger.LogInformation("Actualizando empleado de compañía con ID {CompanyEmployeeId}.", companyEmployee.IdCompanyEmployee);
            try
            {
                var result = await _companyEmployeeRepository.UpdateCompanyEmployee(companyEmployee);
                _logger.LogInformation("Empleado de compañía con ID {CompanyEmployeeId} actualizado exitosamente.", companyEmployee.IdCompanyEmployee);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el empleado de la compañía con ID {CompanyEmployeeId}.", companyEmployee.IdCompanyEmployee);
                throw;
            }
        }

        public async Task<bool> DeleteCompanyEmployee(int id)
        {
            _logger.LogInformation("Eliminando empleado de compañía con ID {CompanyEmployeeId}.", id);
            try
            {
                var result = await _companyEmployeeRepository.DeleteCompanyEmployee(id);
                if (result)
                {
                    _logger.LogInformation("Empleado de compañía con ID {CompanyEmployeeId} eliminado exitosamente.", id);
                }
                else
                {
                    _logger.LogWarning("No se pudo eliminar el empleado de compañía con ID {CompanyEmployeeId}.", id);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el empleado de la compañía con ID {CompanyEmployeeId}.", id);
                throw;
            }
        }
    }
}
