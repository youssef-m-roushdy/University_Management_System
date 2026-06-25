using MediatR;
using University_Management_System.Application.Dtos.InstructorDtos;

namespace University_Management_System.Application.Commands.Instructors
{
    public class AddInstructorToExistingUserCommand : IRequest<InstructorDto>
    {
        public AddInstructorToExistingUserDto Dto { get; set; } = null!;
    }
}