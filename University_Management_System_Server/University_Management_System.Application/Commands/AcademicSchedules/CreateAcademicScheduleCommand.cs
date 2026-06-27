using MediatR;
using University_Management_System.Application.Dtos.AcademicScheduleDtos;

namespace University_Management_System.Application.Commands.AcademicSchedules
{
    public class CreateAcademicScheduleCommand : IRequest<AcademicScheduleDto>
    {
        public CreateAcademicScheduleDto Dto { get; set; } = null!;
        public string AdminId { get; set; } = string.Empty;
    }
}