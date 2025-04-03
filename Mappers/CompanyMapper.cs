using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskIcosoftBackend.Dtos.Companys;
using TaskIcosoftBackend.Models;

namespace TaskIcosoftBackend.Mappers
{
    public static class CompanyMapper
    {
        public static CompanyDto ToDto(this Company company)
        {
            return new CompanyDto
            {
                IdCompany = company.IdCompany,
                CompanyFiscalName = company.CompanyFiscalName,
                CompanyComercialName = company.CompanyComercialName,
                Email = company.Email,
                CompanyAddress = company.CompanyAddress,
                IdCart = company.IdCart,
                CompanyPhone = company.CompanyPhone,
                CreatedAt = company.CreatedAt,
                UpdatedAt = company.UpdatedAt,
                Status = company.Status
            };
        }

        public static Company ToModel(this CreateCompanyDto createCompanyDto)
        {
            return new Company
            {
                CompanyFiscalName = createCompanyDto.CompanyFiscalName,
                CompanyComercialName = createCompanyDto.CompanyComercialName,
                Email = createCompanyDto.Email,
                CompanyAddress = createCompanyDto.CompanyAddress,
                IdCart = createCompanyDto.IdCart,
                CompanyPhone = createCompanyDto.CompanyPhone,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Status = true
            };
        }  

        
    }
}