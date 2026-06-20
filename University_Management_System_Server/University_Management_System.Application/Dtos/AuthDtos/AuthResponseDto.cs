using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.AdminDtos;
using University_Management_System.Application.Dtos.AssistantDtos;
using University_Management_System.Application.Dtos.InstructorDtos;
using University_Management_System.Application.Dtos.StudentDtos;

namespace University_Management_System.Application.Dtos.AuthDtos
{
    public class AuthResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime AccessTokenExpiry { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        
        // Role profiles (null if not applicable)
        public StudentProfileDto? StudentProfile { get; set; }
        public AdminProfileDto? AdminProfile { get; set; }
        public InstructorProfileDto? InstructorProfile { get; set; }
        public AssistantProfileDto? AssistantProfile { get; set; }
    }
}