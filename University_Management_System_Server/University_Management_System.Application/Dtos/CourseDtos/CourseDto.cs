using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Dtos.CourseDtos
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Credits { get; set; }
        public CourseStatus Status { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int PrerequisitesCount { get; set; }
        public int DependenciesCount { get; set; }
    }
}