using MediatR;
using University_Management_System.Application.Dtos.InstructorDtos;

namespace University_Management_System.Application.Commands.Instructors
{
    public class UpdateInstructorCommand : IRequest<InstructorDto>
    {
        public string Id { get; set; } = string.Empty;
        public UpdateInstructorDto Dto { get; set; } = null!;
    }
}