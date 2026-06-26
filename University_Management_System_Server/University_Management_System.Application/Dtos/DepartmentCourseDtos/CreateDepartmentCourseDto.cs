using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Dtos.DepartmentCourseDtos
{
    public class CreateDepartmentCourseDto
    {
        public int DepartmentId { get; set; }
        public int CourseId { get; set; }
        public CourseRole Role { get; set; }
    }
}