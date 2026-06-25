using MediatR;
using University_Management_System.Application.Dtos.AssistantDtos;

namespace University_Management_System.Application.Commands.Assistants
{
    public class UpdateAssistantCommand : IRequest<AssistantDto>
    {
        public string Id { get; set; } = string.Empty;
        public UpdateAssistantDto Dto { get; set; } = null!;
    }
}