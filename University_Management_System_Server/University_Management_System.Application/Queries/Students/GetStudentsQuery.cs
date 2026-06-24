using MediatR;
using University_Management_System.Application.Dtos.StudentDtos;
using University_Management_System.Domain.Queries.StudentQueries;

namespace University_Management_System.Application.Queries.Students
{
    public class GetStudentsQuery : IRequest<(IEnumerable<StudentDto> Data, int TotalCount)>
    {
        public StudentFilterQueries Query { get; set; } = new StudentFilterQueries();
    }
}