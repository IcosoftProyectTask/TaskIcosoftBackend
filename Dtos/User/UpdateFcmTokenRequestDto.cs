using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskIcosoftBackend.Dtos.User
{
    public class UpdateFcmTokenRequestDto
    {
        public int UserId { get; set; }
        public string? FcmToken { get; set; }
    }
}