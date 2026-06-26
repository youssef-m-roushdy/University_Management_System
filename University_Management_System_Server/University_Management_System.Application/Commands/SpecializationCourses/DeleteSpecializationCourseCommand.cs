using MediatR;

namespace University_Management_System.Application.Commands.SpecializationCourses
{
    public class DeleteSpecializationCourseCommand : IRequest<bool>
    {
        public int SpecializationId { get; set; }
        public int CourseId { get; set; }
    }
}