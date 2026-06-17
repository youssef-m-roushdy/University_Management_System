using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Dtos.AuthDtos
{
    public class UserInfoDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string ProfilePicture { get; set; } = string.Empty;
        public string AcademicCode { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public Levels? Level { get; set; }
        public int? TotalCredits { get; set; }
        public int? AllowedCredits { get; set; }
        public decimal? TotalGPA { get; set; }
        public string? Specialization { get; set; }
        public int? DepartmentId { get; set; }
        public string? Department { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}