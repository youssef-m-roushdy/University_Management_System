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
        // Token Generation
        Task<string> GenerateAccessTokenAsync(User user);
        Task<string> GenerateRefreshTokenAsync(string userId);
        DateTime GetAccessTokenExpiryTime();

        // Token Validation
        Task<bool> ValidateRefreshTokenAsync(string refreshToken, string userId);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

        // Token Revocation
        Task RevokeRefreshTokenAsync(string refreshToken);
        Task RevokeAllUserRefreshTokensAsync(string userId);

        // Token Retrieval
        Task<RefreshToken> GetRefreshTokenAsync(string refreshToken);

        // Token Refresh
        Task<string> RefreshAccessTokenAsync(string refreshToken, string userId);
        Task<(string AccessToken, string RefreshToken)> RefreshTokensAsync(string refreshToken, string userId);

        // Maintenance
        Task CleanupExpiredRefreshTokensAsync();
    }
}