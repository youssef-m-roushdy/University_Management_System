namespace University_Management_System.Application.Dtos.CoursePrerequisiteDtos
{
    public class CreateCoursePrerequisiteDto
    {
        public int CourseId { get; set; }
        public int PrerequisiteCourseId { get; set; }
    }
}