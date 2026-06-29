using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using University_Management_System.Application.Contracts;
using University_Management_System.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using University_Management_System.Shared.Settings;
using Microsoft.EntityFrameworkCore;

namespace University_Management_System.Infrastructure.Presistence.Services
{
    public class JwtService : IJwtService
    {
        private readonly UniversityDbContext _context;
        private readonly JwtSettings _settings;
        private readonly UserManager<User> _userManager;

        public JwtService(IOptions<JwtSettings> settings, UserManager<User> userManager, UniversityDbContext context)
        {
            _settings = settings.Value;
            _userManager = userManager;
            _context = context;
        }

        public async Task<string> GenerateAccessTokenAsync(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name, user.UserName ?? string.Empty),
                new(ClaimTypes.Email, user.Email ?? string.Empty),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat,
                    DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64)
            };

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_settings.AccessTokenExpirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> GenerateRefreshTokenAsync(string userId)
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                UserId = userId,
                ExpiresAt = DateTime.UtcNow.AddDays(_settings.RefreshTokenExpirationDays),
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken.Token;
        }

        public DateTime GetAccessTokenExpiryTime()
        {
            return DateTime.UtcNow.AddMinutes(_settings.AccessTokenExpirationMinutes);
        }

        public async Task<bool> ValidateRefreshTokenAsync(string refreshToken, string userId)
        {
            var token = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken 
                                           && rt.UserId == userId 
                                           && !rt.IsRevoked);

            if (token == null)
                return false;

            if (token.ExpiresAt <= DateTime.UtcNow)
                return false;

            return true;
        }

        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            var token = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (token != null)
            {
                token.IsRevoked = true;
                token.RevokedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task RevokeAllUserRefreshTokensAsync(string userId)
        {
            var tokens = await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && !rt.IsRevoked)
                .ToListAsync();

            foreach (var token in tokens)
            {
                token.IsRevoked = true;
                token.RevokedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken> GetRefreshTokenAsync(string refreshToken)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);
        }

        public async Task CleanupExpiredRefreshTokensAsync()
        {
            var expiredTokens = await _context.RefreshTokens
                .Where(rt => rt.ExpiresAt <= DateTime.UtcNow || rt.IsRevoked)
                .ToListAsync();

            if (expiredTokens.Any())
            {
                _context.RefreshTokens.RemoveRange(expiredTokens);
                await _context.SaveChangesAsync();
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            // When refreshing tokens we only need to verify the signature and extract claims.
            // Issuer/Audience validation is intentionally disabled here because:
            //   1. We are not authorizing a request — just identifying the user.
            //   2. Old tokens may have been issued without iss/aud claims.
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey)),
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            
            if (securityToken is not JwtSecurityToken jwtSecurityToken || 
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        public async Task<string> RefreshAccessTokenAsync(string refreshToken, string userId)
        {
            var isValid = await ValidateRefreshTokenAsync(refreshToken, userId);
            if (!isValid)
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found.");

            await RevokeRefreshTokenAsync(refreshToken);

            var newAccessToken = await GenerateAccessTokenAsync(user);
            await GenerateRefreshTokenAsync(userId); // Generate new refresh token

            return newAccessToken;
        }

        public async Task<(string AccessToken, string RefreshToken)> RefreshTokensAsync(string refreshToken, string userId)
        {
            var isValid = await ValidateRefreshTokenAsync(refreshToken, userId);
            if (!isValid)
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found.");

            await RevokeRefreshTokenAsync(refreshToken);

            var newAccessToken = await GenerateAccessTokenAsync(user);
            var newRefreshToken = await GenerateRefreshTokenAsync(userId);

            return (newAccessToken, newRefreshToken);
        }

        // ✅ NEW: Get user ID from expired token
        public string GetUserIdFromExpiredToken(string token)
        {
            var principal = GetPrincipalFromExpiredToken(token);
            return principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        // ✅ NEW: Validate token without checking expiry
        public bool ValidateToken(string token, bool validateLifetime = true)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey)),
                    ValidateLifetime = validateLifetime,
                    ValidIssuer = _settings.Issuer,
                    ValidAudience = _settings.Audience,
                    ClockSkew = TimeSpan.Zero
                };

                tokenHandler.ValidateToken(token, validationParameters, out _);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}