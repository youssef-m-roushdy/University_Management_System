using MediatR;
using University_Management_System.Application.Dtos.AssistantDtos;
using University_Management_System.Domain.Queries.AssistantQueries;

namespace University_Management_System.Application.Queries.Assistants
{
    public class GetDepartmentAssistantsQuery : IRequest<(IEnumerable<AssistantDto> Data, int TotalCount)>
    {
        public int DepartmentId { get; set; }
        public AssistantDepartmentQueries Query { get; set; } = new AssistantDepartmentQueries();
    }
}