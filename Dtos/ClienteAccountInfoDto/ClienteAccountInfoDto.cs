using System;

namespace TaskIcosoftBackend.Dtos.ClienteAccountInfoDto
{
    public class ClienteAccountInfoDto
    {
        public int IdClienteAccountInfo { get; set; }
        public string Client { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string AppPassword { get; set; } = string.Empty;
        public string Vin { get; set; } = string.Empty;
        public DateTime Date1 { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Status { get; set; }
    }
}