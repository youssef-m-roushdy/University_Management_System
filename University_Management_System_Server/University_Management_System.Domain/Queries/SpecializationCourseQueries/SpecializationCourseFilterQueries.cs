using University_Management_System.Domain.Enums;

namespace University_Management_System.Domain.Queries.SpecializationCourseQueries
{
    public class SpecializationCourseFilterQueries : SearchablePaginationQuery
    {
        public int? SpecializationId { get; set; }
        public int? CourseId { get; set; }
        public SpecializationCourseRole? Role { get; set; }
        public string? CourseName { get; set; }
        public string? CourseCode { get; set; }
        public int? MinCredits { get; set; }
        public int? MaxCredits { get; set; }
        public bool? HasPrerequisites { get; set; }
    }
}