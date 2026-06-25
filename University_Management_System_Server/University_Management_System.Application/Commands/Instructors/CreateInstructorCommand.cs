using MediatR;
using University_Management_System.Application.Dtos.InstructorDtos;

namespace University_Management_System.Application.Commands.Instructors
{
    public class CreateInstructorCommand : IRequest<InstructorDto>
    {
        public CreateInstructorDto Dto { get; set; } = null!;
    }
}