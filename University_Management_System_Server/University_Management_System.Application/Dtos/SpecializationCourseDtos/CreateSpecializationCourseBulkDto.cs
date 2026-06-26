using System.Collections.Generic;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Dtos.SpecializationCourseDtos
{
    public class CreateSpecializationCourseBulkDto
    {
        public int SpecializationId { get; set; }
        public List<int> CourseIds { get; set; } = new List<int>();
        public SpecializationCourseRole Role { get; set; }
    }
}