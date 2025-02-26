using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskIcosoftBackend.Dtos.Companys
{
    public class CompanyDto
    {
        public int IdCompany { get; set; }
        public string CompanyFiscalName { get; set; } = string.Empty;
        public string CompanyComercialName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CompanyAddress { get; set; } = string.Empty;
        public string IdCart { get; set; } = string.Empty;
        public string CompanyPhone { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool Status { get; set; }
        

        
    }
}