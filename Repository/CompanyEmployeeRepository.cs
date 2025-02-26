using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskIcosoftBackend.Data;
using TaskIcosoftBackend.Models;

namespace TaskIcosoftBackend.Repository
{
    public class CompanyEmployeeRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<CompanyEmployeeRepository> _logger;

        public CompanyEmployeeRepository(DataContext context, ILogger<CompanyEmployeeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<CompanyEmployees> CreateCompanyEmployee(CompanyEmployees companyEmployee)
        {
            try
            {
                await _context.CompanyEmployees.AddAsync(companyEmployee);
                await _context.SaveChangesAsync();
                return companyEmployee;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al crear el empleado de la compañía.");
                throw new ApplicationException("Error al crear el empleado de la compañía.", e);
            }
        }

        public async Task<CompanyEmployees> GetCompanyEmployeeById(int id)
        {
            try
            {
                return await _context.CompanyEmployees
                    .Where(c => c.Status == true) // Solo empleados activos
                    .FirstOrDefaultAsync(c => c.IdCompanyEmployee == id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error al obtener el empleado de la compañía con ID {id}.");
                throw new ApplicationException($"Error al obtener el empleado de la compañía con ID {id}.", e);
            }
        }

        public async Task<CompanyEmployees> GetCompanyEmployeeByIdCompany(int id)
        {
            try
            {
                return await _context.CompanyEmployees
                    .Where(c => c.Status == true) // Solo empleados activos
                    .FirstOrDefaultAsync(c => c.IdCompany == id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error al obtener el empleado de la compañía con ID {id}.");
                throw new ApplicationException($"Error al obtener el empleado de la compañía con ID {id}.", e);
            }
        }

        public async Task<List<CompanyEmployees>> GetCompanyEmployees()
        {
            try
            {
                return await _context.CompanyEmployees
                    .Where(c => c.Status == true) // Solo empleados activos
                    .ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al obtener los empleados de la compañía.");
                throw new ApplicationException("Error al obtener los empleados de la compañía.", e);
            }
        }

        public async Task<CompanyEmployees> UpdateCompanyEmployee(CompanyEmployees companyEmployee)
        {
            try
            {
                _context.CompanyEmployees.Update(companyEmployee);
                await _context.SaveChangesAsync();
                return companyEmployee;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error al actualizar el empleado de la compañía con ID {companyEmployee.IdCompanyEmployee}.");
                throw new ApplicationException($"Error al actualizar el empleado de la compañía con ID {companyEmployee.IdCompanyEmployee}.", e);
            }
        }

        public async Task<bool> DeleteCompanyEmployee(int id)
        {
            try
            {
                var companyEmployee = await _context.CompanyEmployees
                    .Where(c => c.Status == true) // Solo empleados activos
                    .FirstOrDefaultAsync(c => c.IdCompanyEmployee == id);
                if (companyEmployee == null)
                {
                    throw new ApplicationException($"No se encontró el empleado de la compañía con ID {id}.");
                }
                companyEmployee.Status = false;
                _context.CompanyEmployees.Update(companyEmployee);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error al eliminar el empleado de la compañía con ID {id}.");
                throw new ApplicationException($"Error al eliminar el empleado de la compañía con ID {id}.", e);    
            }
        }


    }
}