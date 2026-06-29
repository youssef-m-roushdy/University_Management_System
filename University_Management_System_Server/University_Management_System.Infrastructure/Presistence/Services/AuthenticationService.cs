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
using University_Management_System.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using University_Management_System.Shared.Exceptions;
using University_Management_System.Domain.Enums;
using University_Management_System.Application.Dtos.AuthDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Settings;
using University_Management_System.Application.Dtos.StudentDtos;
using University_Management_System.Application.Dtos.AdminDtos;
using University_Management_System.Application.Dtos.InstructorDtos;
using University_Management_System.Application.Dtos.AssistantDtos;

namespace University_Management_System.Infrastructure.Presistence.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IJwtService _jwtService;
        private readonly JwtSettings _settings;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly EmailSettings _emailSettings;
        private readonly UniversityDbContext _context;

        public AuthenticationService(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IJwtService jwtService,
            IOptions<JwtSettings> settings,
            IOptions<EmailSettings> emailSettings,
            IUnitOfWork unitOfWork,
            IEmailService emailService,
            UniversityDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
            _settings = settings.Value;
            _unitOfWork = unitOfWork;
            _emailSettings = emailSettings.Value;
            _context = context;
            _emailService = emailService;
        }

        // ────────────────────────────────────────────────────────────────────────
        // LOGIN - ✅ IsActive check added
        // ────────────────────────────────────────────────────────────────────────
        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            // 1. Get user with projection (only active users)
            var userQuery = from u in _userManager.Users
                            where u.Email == dto.Email && u.IsActive == true
                            select new
                            {
                                User = u,
                                Student = u.Student != null ? new
                                {
                                    u.Student.AcademicCode,
                                    u.Student.Level,
                                    u.Student.TotalGPA,
                                    u.Student.DepartmentId,
                                    DepartmentName = u.Student.Department.Name,
                                    u.Student.SpecializationId,
                                    SpecializationName = u.Student.Specialization.Name
                                } : null,
                                Instructor = u.Instructor != null ? new
                                {
                                    u.Instructor.DepartmentId,
                                    DepartmentName = u.Instructor.Department.Name
                                } : null,
                                Assistant = u.Assistant != null ? new
                                {
                                    u.Assistant.DepartmentId,
                                    DepartmentName = u.Assistant.Department.Name
                                } : null,
                                HasAdmin = u.Admin != null
                            };

            var userData = await userQuery.FirstOrDefaultAsync();

            if (userData == null)
                throw new NotFoundException($"No active user with email '{dto.Email}'.");

            // 2. Verify password
            if (!await _userManager.CheckPasswordAsync(userData.User, dto.Password))
                throw new UnauthorizedAccessException("Invalid credentials.");

            // 3. Check if email is confirmed
            if (!await _userManager.IsEmailConfirmedAsync(userData.User))
                throw new UnauthorizedAccessException("Email not confirmed. Please verify your email.");

            // 4. Get roles
            var roles = await _userManager.GetRolesAsync(userData.User);

            // 5. Generate tokens
            var accessToken = await _jwtService.GenerateAccessTokenAsync(userData.User);
            var refreshToken = await _jwtService.GenerateRefreshTokenAsync(userData.User.Id);

            // 6. Build response
            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiry = _jwtService.GetAccessTokenExpiryTime(),
                UserId = userData.User.Id,
                Email = userData.User.Email!,
                Name = userData.User.Name,
                ProfilePicture = userData.User.ProfilePicture,
                Roles = roles.ToList(),
                StudentProfile = userData.Student != null ? new StudentProfileDto
                {
                    AcademicCode = userData.Student.AcademicCode,
                    Level = userData.Student.Level,
                    TotalGPA = userData.Student.TotalGPA,
                    DepartmentId = userData.Student.DepartmentId,
                    DepartmentName = userData.Student.DepartmentName,
                    SpecializationId = userData.Student.SpecializationId,
                    SpecializationName = userData.Student.SpecializationName
                } : null,
                InstructorProfile = userData.Instructor != null ? new InstructorProfileDto
                {
                    DepartmentId = userData.Instructor.DepartmentId,
                    DepartmentName = userData.Instructor.DepartmentName
                } : null,
                AssistantProfile = userData.Assistant != null ? new AssistantProfileDto
                {
                    DepartmentId = userData.Assistant.DepartmentId,
                    DepartmentName = userData.Assistant.DepartmentName
                } : null,
                AdminProfile = userData.HasAdmin ? new AdminProfileDto() : null
            };
        }

        // ────────────────────────────────────────────────────────────────────────
        // LOGOUT
        // ────────────────────────────────────────────────────────────────────────
        public async Task LogoutAsync(string userId, string refreshToken)
        {
            // Check if user exists and is active
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || !user.IsActive)
                return; // Silent return for security

            // Revoke the specific refresh token
            await _jwtService.RevokeRefreshTokenAsync(refreshToken);
        }

        // ────────────────────────────────────────────────────────────────────────
        // REFRESH TOKEN - ✅ IsActive check added
        // ────────────────────────────────────────────────────────────────────────
        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken, string userId)
        {
            // 1. Validate refresh token
            var isValid = await _jwtService.ValidateRefreshTokenAsync(refreshToken, userId);
            if (!isValid)
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");

            // 2. Get user
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new NotFoundException($"User with ID '{userId}' not found.");

            // 3. Check if user is active
            if (!user.IsActive)
                throw new UnauthorizedAccessException("Account is deactivated. Please contact support.");

            // 4. Check if email is confirmed
            if (!await _userManager.IsEmailConfirmedAsync(user))
                throw new UnauthorizedAccessException("Email not confirmed.");

            // 5. ⚠️ IMPORTANT: Revoke the old refresh token FIRST
            await _jwtService.RevokeRefreshTokenAsync(refreshToken);

            // 6. Generate new tokens
            var newAccessToken = await _jwtService.GenerateAccessTokenAsync(user);
            var newRefreshToken = await _jwtService.GenerateRefreshTokenAsync(userId);

            // 7. Get roles
            var roles = await _userManager.GetRolesAsync(user);

            // 8. Build response
            return new AuthResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                AccessTokenExpiry = _jwtService.GetAccessTokenExpiryTime(),
                UserId = user.Id,
                Email = user.Email!,
                Name = user.Name,
                ProfilePicture = user.ProfilePicture,
                Roles = roles.ToList()
            };
        }

        // ────────────────────────────────────────────────────────────────────────
        // REVOKE TOKEN - ✅ IsActive check added
        // ────────────────────────────────────────────────────────────────────────
        public async Task RevokeTokenAsync(string refreshToken)
        {
            // Get the token from database
            var token = await _jwtService.GetRefreshTokenAsync(refreshToken);
            if (token == null)
                return; // Silent return

            // Check if user is active
            var user = await _userManager.FindByIdAsync(token.UserId);
            if (user == null || !user.IsActive)
                return; // Silent return

            await _jwtService.RevokeRefreshTokenAsync(refreshToken);
        }

        // ────────────────────────────────────────────────────────────────────────
        // REVOKE ALL TOKENS - ✅ IsActive check added
        // ────────────────────────────────────────────────────────────────────────
        public async Task RevokeAllTokensAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || !user.IsActive)
                throw new UnauthorizedAccessException("Account is deactivated.");

            await _jwtService.RevokeAllUserRefreshTokensAsync(userId);
        }

        // ────────────────────────────────────────────────────────────────────────
        // FORGOT PASSWORD - ✅ IsActive check added
        // ────────────────────────────────────────────────────────────────────────
        public async Task ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            
            // Silent return for security - never reveal if email exists or account is inactive
            if (user == null || !user.IsActive)
                return;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Uri.EscapeDataString(token);
            var encodedEmail = Uri.EscapeDataString(email);
            var resetLink = $"{_emailSettings.FrontendBaseUrl}/reset-password?email={encodedEmail}&token={encodedToken}";

            await _emailService.SendPasswordResetEmailAsync(
                to: email,
                displayName: user.Name ?? user.UserName ?? "Student",
                resetLink: resetLink
            );
        }

        // ────────────────────────────────────────────────────────────────────────
        // RESET PASSWORD
        // ────────────────────────────────────────────────────────────────────────
        public async Task<string> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email)
                ?? throw new NotFoundException($"No user with email '{email}'.");

            // ✅ Check if user is active
            if (!user.IsActive)
                throw new UnauthorizedAccessException("Account is deactivated. Please contact support.");

            var decodedToken = Uri.UnescapeDataString(token);
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, newPassword);

            if (!result.Succeeded)
                return string.Join(" | ", result.Errors.Select(e => e.Description));

            // Send confirmation email
            await _emailService.SendPasswordChangedConfirmationAsync(
                to: email,
                displayName: user.Name ?? user.UserName ?? "Student"
            );

            return "Password reset successfully.";
        }

        // ────────────────────────────────────────────────────────────────────────
        // CHANGE PASSWORD - ✅ IsActive check added
        // ────────────────────────────────────────────────────────────────────────
        public async Task<string> ChangePasswordAsync(string email, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email)
                ?? throw new NotFoundException($"No user with email '{email}'.");

            // ✅ Check if user is active
            if (!user.IsActive)
                throw new UnauthorizedAccessException("Account is deactivated. Please contact support.");

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            if (!result.Succeeded)
                return string.Join(" | ", result.Errors.Select(e => e.Description));

            // Revoke all refresh tokens on password change (security)
            await _jwtService.RevokeAllUserRefreshTokensAsync(user.Id);

            // Send confirmation email
            await _emailService.SendPasswordChangedConfirmationAsync(
                to: email,
                displayName: user.Name ?? user.UserName ?? "Student"
            );

            return "Password changed successfully.";
        }

        // ────────────────────────────────────────────────────────────────────────
        // VERIFY EMAIL - ✅ IsActive check added
        // ────────────────────────────────────────────────────────────────────────
        public async Task<string> VerifyEmailAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email)
                ?? throw new NotFoundException($"User with email '{email}' not found.");

            // ✅ Check if user is active
            if (!user.IsActive)
                throw new UnauthorizedAccessException("Account is deactivated. Please contact support.");

            if (user.EmailConfirmed)
                return "Email already verified.";

            var decodedToken = Uri.UnescapeDataString(token);
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (!result.Succeeded)
                return string.Join(" | ", result.Errors.Select(e => e.Description));

            return "Email verified successfully.";
        }

        // ────────────────────────────────────────────────────────────────────────
        // RESEND VERIFICATION EMAIL - ✅ IsActive check added
        // ────────────────────────────────────────────────────────────────────────
        public async Task ResendVerificationEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            
            // Silent return for security
            if (user == null || !user.IsActive)
                return;

            if (user.EmailConfirmed)
                return; // Already confirmed

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = Uri.EscapeDataString(token);
            var encodedEmail = Uri.EscapeDataString(email);
            var verificationLink = $"{_emailSettings.FrontendBaseUrl}/verify-email?email={encodedEmail}&token={encodedToken}";

            await _emailService.SendEmailVerificationAsync(
                to: email,
                displayName: user.Name ?? user.UserName ?? "Student",
                verificationLink: verificationLink
            );
        }
    }
}