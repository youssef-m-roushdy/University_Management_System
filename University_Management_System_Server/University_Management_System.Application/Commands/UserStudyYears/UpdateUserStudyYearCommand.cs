using University_Management_System.Application.Dtos.UserStudyYearDtos;
using MediatR;
using University_Management_System.Shared.Respones;

namespace University_Management_System.Application.Commands.UserStudyYears
{
    public class UpdateUserStudyYearCommand : IRequest<Response<UserStudyYearDto>>
    {
        public int Id { get; set; }
        public UpdateUserStudyYearDto Dto { get; set; } = null!;

        public UpdateUserStudyYearCommand(int id, UpdateUserStudyYearDto dto)
        {
            Id = id;
            Dto = dto;
        }
    }
}
