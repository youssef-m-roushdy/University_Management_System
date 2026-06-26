using MediatR;

namespace University_Management_System.Application.Commands.CoursePrerequisites
{
    public class DeleteCoursePrerequisiteCommand : IRequest<bool>
    {
        public int CourseId { get; set; }
        public int PrerequisiteCourseId { get; set; }
    }
}