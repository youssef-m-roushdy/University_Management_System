using MediatR;
using University_Management_System.Application.Dtos.StudentDtos;
using University_Management_System.Domain.Queries.StudentQueries;

namespace University_Management_System.Application.Queries.Students
{
    public class GetDepartmentStudentsQuery : IRequest<(IEnumerable<StudentDto> Data, int TotalCount)>
    {
        public int DepartmentId { get; set; }
        public StudentDepartmentQueries Query { get; set; } = new StudentDepartmentQueries();
    }
}