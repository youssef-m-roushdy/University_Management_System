using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Dtos.StudentDtos
{
    public class AddStudentToExistingUserDto
    {
        public string UserEmail { get; set; } = string.Empty;
        public string AcademicCode { get; set; } = string.Empty;
        public Levels Level { get; set; }
        public int TotalCredits { get; set; }
        public int AllowedCredits { get; set; }
        public decimal TotalGPA { get; set; } = 0.0m;
        public int DepartmentId { get; set; }
        public int? SpecializationId { get; set; }
        // ✅ No User fields (Name, Email, Password, etc.) - user already exists
    }
}