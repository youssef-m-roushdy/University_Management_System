using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.AuthDtos;
using University_Management_System.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace University_Management_System.Application.Dtos.AuthDtos
{
    public class AuthResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime AccessTokenExpiry { get; set; }
        public UserInfoDto User { get; set; }
    }
}