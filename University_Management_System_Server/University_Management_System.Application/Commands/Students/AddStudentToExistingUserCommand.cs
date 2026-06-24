using MediatR;
using University_Management_System.Application.Dtos.StudentDtos;

namespace University_Management_System.Application.Commands.Students
{
    public class AddStudentToExistingUserCommand : IRequest<StudentDto>
    {
        public AddStudentToExistingUserDto Dto { get; set; } = null!;
    }
}