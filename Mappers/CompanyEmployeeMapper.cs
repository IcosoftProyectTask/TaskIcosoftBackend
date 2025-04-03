using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskIcosoftBackend.Dtos.Companys;
using TaskIcosoftBackend.Models;

namespace TaskIcosoftBackend.Mappers
{
    public static class CompanyEmployeeMapper
    {
        public static CompanyEmployeeDto ToDto(this CompanyEmployees companyEmployee)
        {
            return new CompanyEmployeeDto
            {
                IdCompanyEmployee = companyEmployee.IdCompanyEmployee,
                NameEmployee = companyEmployee.NameEmployee,
                FirstSurname = companyEmployee.FirstSurname,
                SecondSurname = companyEmployee.SecondSurname,
                IdCompany = companyEmployee.IdCompany,
                CreatedAt = companyEmployee.CreatedAt,
                UpdatedAt = companyEmployee.UpdatedAt,
                Status = companyEmployee.Status
            };
        }

        public static CompanyEmployees ToModel(this CreateCompanyEmployeeDto createCompanyEmployeeDto)
        {
            return new CompanyEmployees
            {
                NameEmployee = createCompanyEmployeeDto.NameEmployee,
                FirstSurname = createCompanyEmployeeDto.FirstSurname,
                SecondSurname = createCompanyEmployeeDto.SecondSurname,
                IdCompany = createCompanyEmployeeDto.IdCompany,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Status = true
            };
        }
        
    }
}