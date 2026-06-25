using MediatR;
using University_Management_System.Application.Dtos.AssistantDtos;

namespace University_Management_System.Application.Queries.Assistants
{
    public class GetAssistantQuery : IRequest<AssistantDto?>
    {
        public string Id { get; set; } = string.Empty;
    }
}