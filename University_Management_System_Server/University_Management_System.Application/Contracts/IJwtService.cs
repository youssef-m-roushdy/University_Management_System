using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using University_Management_System.Domain.Entities.Identity;

namespace University_Management_System.Application.Contracts
{
    public interface IJwtService
    {
        Task<string> GenerateAccessTokenAsync(User user);
        RefreshToken GenerateRefreshToken();
    }
}