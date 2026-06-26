using University_Management_System.Domain.Enums;

namespace University_Management_System.Domain.Queries.DepartmentCourseQueries
{
    public class DepartmentCourseFilterQueries : SearchablePaginationQuery
    {
        public CourseRole? Role { get; set; }
        public int? DepartmentId { get; set; }
        public string? CourseName { get; set; }
        public string? CourseCode { get; set; }
        public int? MinCredits { get; set; }
        public int? MaxCredits { get; set; }
    }
}