
using University_Management_System.Application.Dtos.AuthDtos;
using University_Management_System.Shared.Respones;

namespace University_Management_System.Application.Contracts
{
    public interface IAuthenticationService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto, string role = "Student");
        Task<AuthResponseDto> RegisterStudentAsync(int departmentId, RegisterStudentDto registerStudentDto);
        Task ForgotPasswordAsync(string email);                                          // ← NEW
        Task<string> ResetPasswordAsync(string email, string token, string newPassword); // ← UPDATED
        Task<string> ChangePasswordAsync(string email, string currentPassword, string newPassword);
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
        Task RevokeTokenAsync(string refreshToken);
    }
}
