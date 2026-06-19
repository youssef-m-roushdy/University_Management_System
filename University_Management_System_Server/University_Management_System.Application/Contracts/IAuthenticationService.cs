
using University_Management_System.Application.Dtos.AuthDtos;

namespace University_Management_System.Application.Contracts
{
    public interface IAuthenticationService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task ForgotPasswordAsync(string email);                                          // ← NEW
        Task<string> ResetPasswordAsync(string email, string token, string newPassword); // ← UPDATED
        Task<string> ChangePasswordAsync(string email, string currentPassword, string newPassword);
    }
}
