using MediatR;
using University_Management_System.Application.Dtos.InstructorDtos;
using University_Management_System.Domain.Queries.InstructorQueries;

namespace University_Management_System.Application.Queries.Instructors
{
    public class GetInstructorsQuery : IRequest<(IEnumerable<InstructorDto> Data, int TotalCount)>
    {
        public InstructorFilterQueries Query { get; set; } = new InstructorFilterQueries();
    }
}