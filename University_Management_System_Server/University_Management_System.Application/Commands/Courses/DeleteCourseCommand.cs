using MediatR;

namespace University_Management_System.Application.Commands.Courses
{
    public class DeleteCourseCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}