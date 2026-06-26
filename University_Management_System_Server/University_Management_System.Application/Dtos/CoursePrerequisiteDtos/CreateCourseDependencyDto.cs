namespace University_Management_System.Application.Dtos.CoursePrerequisiteDtos
{
    public class CreateCourseDependencyDto
    {
        public int CourseId { get; set; }              // The course that depends on the prerequisite
        public int PrerequisiteCourseId { get; set; }  // The course that is required
    }
}