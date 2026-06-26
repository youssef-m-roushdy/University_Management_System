using MediatR;

namespace University_Management_System.Application.Commands.CoursePrerequisites
{
    public class DeleteCourseDependencyCommand : IRequest<bool>
    {
        public int CourseId { get; set; }           // The course that depends on others
        public int DependencyCourseId { get; set; } // The course that is a prerequisite
    }
}