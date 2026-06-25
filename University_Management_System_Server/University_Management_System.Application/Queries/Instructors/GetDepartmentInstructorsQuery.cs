using MediatR;
using University_Management_System.Application.Dtos.InstructorDtos;
using University_Management_System.Domain.Queries.InstructorQueries;

namespace University_Management_System.Application.Queries.Instructors
{
    public class GetDepartmentInstructorsQuery : IRequest<(IEnumerable<InstructorDto> Data, int TotalCount)>
    {
        public int DepartmentId { get; set; }
        public InstructorDepartmentQueries Query { get; set; } = new InstructorDepartmentQueries();
    }
}