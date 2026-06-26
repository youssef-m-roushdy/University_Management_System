using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Dtos.SpecializationCourseDtos
{
    public class CreateSpecializationCourseDto
    {
        public int SpecializationId { get; set; }
        public int CourseId { get; set; }
        public SpecializationCourseRole Role { get; set; }
    }
}