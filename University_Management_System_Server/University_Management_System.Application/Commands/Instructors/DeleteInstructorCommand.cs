using MediatR;

namespace University_Management_System.Application.Commands.Instructors
{
    public class DeleteInstructorCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
    }
}