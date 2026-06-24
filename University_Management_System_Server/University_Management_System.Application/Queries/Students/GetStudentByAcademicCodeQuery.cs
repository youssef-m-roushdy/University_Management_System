using MediatR;
using University_Management_System.Application.Dtos.StudentDtos;

namespace University_Management_System.Application.Queries.Students
{
    public class GetStudentByAcademicCodeQuery : IRequest<StudentDto?>
    {
        public string AcademicCode { get; set; } = string.Empty;
    }
}