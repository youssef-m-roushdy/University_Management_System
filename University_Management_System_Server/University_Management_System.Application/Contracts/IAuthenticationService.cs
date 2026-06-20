
using University_Management_System.Application.Dtos.AuthDtos;

namespace University_Management_System.Application.Contracts
{
    public interface IAuthenticationService
    {
        // Auth
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task LogoutAsync(string userId, string refreshToken);  // userId from token
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken, string userId);  // userId from expired token
        
        // Token Revocation
        Task RevokeTokenAsync(string refreshToken);  // userId from token (verified)
        Task RevokeAllTokensAsync(string userId);    // userId from token

        // Password Management
        Task ForgotPasswordAsync(string email);
        Task<string> ResetPasswordAsync(string email, string token, string newPassword);
        Task<string> ChangePasswordAsync(string email, string currentPassword, string newPassword);

        // Email Verification
        Task<string> VerifyEmailAsync(string email, string token);
        Task ResendVerificationEmailAsync(string email);
    }
}