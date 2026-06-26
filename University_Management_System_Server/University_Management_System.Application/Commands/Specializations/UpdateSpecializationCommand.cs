using MediatR;
using University_Management_System.Application.Dtos.SpecializationDtos;

namespace University_Management_System.Application.Commands.Specializations
{
    public class UpdateSpecializationCommand : IRequest<SpecializationDto>
    {
        public int Id { get; set; }
        public UpdateSpecializationDto Dto { get; set; } = null!;
    }
}