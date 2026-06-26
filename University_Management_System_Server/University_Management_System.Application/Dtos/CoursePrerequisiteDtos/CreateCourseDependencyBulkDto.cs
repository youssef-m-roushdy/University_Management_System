using System.Collections.Generic;

namespace University_Management_System.Application.Dtos.CoursePrerequisiteDtos
{
    public class CreateCourseDependencyBulkDto
    {
        public int PrerequisiteCourseId { get; set; }  // The course that is required
        public List<int> DependentCourseIds { get; set; } = new List<int>(); // Courses that depend on it
    }
}