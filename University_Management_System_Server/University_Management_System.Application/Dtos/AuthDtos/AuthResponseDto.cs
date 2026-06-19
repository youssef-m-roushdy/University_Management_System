using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.AuthDtos;
using University_Management_System.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using University_Management_System.Application.Dtos.StudentDtos;
using University_Management_System.Application.Dtos.AdminDtos;
using University_Management_System.Application.Dtos.InstructorDtos;
using University_Management_System.Application.Dtos.AssistantDtos;

namespace University_Management_System.Application.Dtos.AuthDtos
{
    public class AuthResponseDto
    {
        // Essential Tokens
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime AccessTokenExpiry { get; set; }

        // User Information (Frontend needs this immediately)
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; }

        // Authorization
        public List<string> Roles { get; set; } = new List<string>();

        // Role-Specific Data (Optional but useful)
        public StudentProfileDto? StudentProfile { get; set; }
        public AdminProfileDto? AdminProfile { get; set; }
        public InstructorProfileDto? InstructorProfile { get; set; }
        public AssistantProfileDto? AssistantProfile { get; set; }
    }
}