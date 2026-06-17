using University_Management_System.Application.Dtos.UserStudyYearDtos;
using MediatR;
using University_Management_System.Shared.Respones;

namespace University_Management_System.Application.Commands.UserStudyYears
{
    public class CreateUserStudyYearCommand : IRequest<Response<UserStudyYearDto>>
    {
        public CreateUserStudyYearDto Dto { get; set; } = null!;

        public CreateUserStudyYearCommand(CreateUserStudyYearDto dto)
        {
            Dto = dto;
        }
    }
}
