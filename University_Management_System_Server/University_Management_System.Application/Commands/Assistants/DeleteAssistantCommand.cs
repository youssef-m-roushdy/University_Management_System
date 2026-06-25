using MediatR;

namespace University_Management_System.Application.Commands.Assistants
{
    public class DeleteAssistantCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
    }
}