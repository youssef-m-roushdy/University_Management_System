using MediatR;
using University_Management_System.Application.Dtos.AssistantDtos;

namespace University_Management_System.Application.Commands.Assistants
{
    public class CreateAssistantCommand : IRequest<AssistantDto>
    {
        public CreateAssistantDto Dto { get; set; } = null!;
    }
}