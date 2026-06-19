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
        private readonly EmailSettings _emailSettings; // ← ADD
        private readonly UniversityDbContext _context; // ← ADD

        public AuthenticationService(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IJwtService jwtService,
            IOptions<JwtSettings> settings,
            IOptions<EmailSettings> emailSettings,  // ← ADD
            IUnitOfWork unitOfWork,
            IEmailService emailService,
            UniversityDbContext context) // ← ADD
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
            _settings = settings.Value;
            _unitOfWork = unitOfWork;
            _emailSettings = emailSettings.Value; // ← ADD
            _context = context; // ← ADD
            _emailService = emailService;
        }

        // AuthService.cs — completed LoginAsync
        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            // 1. Get user with projection
            var userQuery = from u in _userManager.Users
                            where u.Email == dto.Email
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
                throw new NotFoundException($"No user with email '{dto.Email}'.");

            // 2. Verify password
            if (!await _userManager.CheckPasswordAsync(userData.User, dto.Password))
                throw new UnauthorizedAccessException("Invalid credentials.");

            // 3. Get roles
            var roles = await _userManager.GetRolesAsync(userData.User);

            // 4. Generate tokens
            var accessToken = await _jwtService.GenerateAccessTokenAsync(userData.User);
            var refreshToken = await _jwtService.GenerateRefreshTokenAsync(userData.User.Id);

            // 5. Build response
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

      
        // ── Forgot password ────────────────────────────────────────────────────────
        public async Task ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new NotFoundException("No user not found use that email."); // silent — never reveal if email exists

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

        // ── Reset password ─────────────────────────────────────────────────────────
        public async Task<string> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email)
                ?? throw new NotFoundException($"No user with email '{email}'.");

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

        // ── Change password (authorized) ──────────────────────────────────────────
        public async Task<string> ChangePasswordAsync(string email, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email)
                ?? throw new NotFoundException($"No user with email '{email}'.");

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            if (!result.Succeeded)
                return string.Join(" | ", result.Errors.Select(e => e.Description));

            // Send confirmation email
            await _emailService.SendPasswordChangedConfirmationAsync(
                to: email,
                displayName: user.Name ?? user.UserName ?? "Student"
            );

            return "Password changed successfully.";
        }

        // ── Private helpers ────────────────────────────────────────────────────


    }
}
