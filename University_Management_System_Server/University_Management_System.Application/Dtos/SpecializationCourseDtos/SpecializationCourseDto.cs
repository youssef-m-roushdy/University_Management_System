using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Dtos.SpecializationCourseDtos
{
    public class SpecializationCourseDto
    {
        public int Id { get; set; }
        public int SpecializationId { get; set; }
        public string SpecializationName { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public int Credits { get; set; }
        public SpecializationCourseRole Role { get; set; }
        public string? DepartmentName { get; set; }
        public int PrerequisitesCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}