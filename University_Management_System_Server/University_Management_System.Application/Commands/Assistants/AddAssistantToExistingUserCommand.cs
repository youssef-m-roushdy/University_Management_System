using MediatR;
using University_Management_System.Application.Dtos.AssistantDtos;

namespace University_Management_System.Application.Commands.Assistants
{
    public class AddAssistantToExistingUserCommand : IRequest<AssistantDto>
    {
        public AddAssistantToExistingUserDto Dto { get; set; } = null!;
    }
}