using MediatR;
using University_Management_System.Application.Dtos.StudentDtos;

namespace University_Management_System.Application.Queries.Students
{
    public class GetStudentQuery : IRequest<StudentDto?>
    {
        public string Id { get; set; } = string.Empty;
    }
}