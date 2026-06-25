using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Dtos.InstructorDtos
{
    public class InstructorDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Address { get; set; }
        public Gender Gender { get; set; }
        public bool IsActive { get; set; }
        
        // Instructor-specific
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string DepartmentCode { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}