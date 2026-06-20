using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Application.Dtos.AuthDtos
{
    public class LogoutDto
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}