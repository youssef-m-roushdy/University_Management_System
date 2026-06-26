using System.Collections.Generic;

namespace University_Management_System.Application.Dtos.CoursePrerequisiteDtos
{
    public class CreateCoursePrerequisiteBulkDto
    {
        public int CourseId { get; set; }
        public List<int> PrerequisiteCourseIds { get; set; } = new List<int>();
    }
}