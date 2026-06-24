using MediatR;

namespace University_Management_System.Application.Commands.Students
{
    public class DeleteStudentCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
    }
}