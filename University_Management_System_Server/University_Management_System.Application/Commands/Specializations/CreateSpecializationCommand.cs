using MediatR;
using University_Management_System.Application.Dtos.SpecializationDtos;

namespace University_Management_System.Application.Commands.Specializations
{
    public class CreateSpecializationCommand : IRequest<SpecializationDto>
    {
        public CreateSpecializationDto Dto { get; set; } = null!;
    }
}