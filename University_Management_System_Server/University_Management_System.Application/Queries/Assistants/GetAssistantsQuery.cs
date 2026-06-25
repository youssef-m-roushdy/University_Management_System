using MediatR;
using University_Management_System.Application.Dtos.AssistantDtos;
using University_Management_System.Domain.Queries.AssistantQueries;

namespace University_Management_System.Application.Queries.Assistants
{
    public class GetAssistantsQuery : IRequest<(IEnumerable<AssistantDto> Data, int TotalCount)>
    {
        public AssistantFilterQueries Query { get; set; } = new AssistantFilterQueries();
    }
}