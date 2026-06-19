using University_Management_System.Application.Dtos.StudentStudyYearDtos;
using MediatR;
using University_Management_System.Shared.Responses;

namespace University_Management_System.Application.Commands.StudentStudyYears
{
    public class UpdateStudentStudyYearCommand : IRequest<ApiResponse<StudentStudyYearDto>>
    {
        public int Id { get; set; }
        public UpdateStudentStudyYearDto Dto { get; set; } = null!;

        public UpdateStudentStudyYearCommand(int id, UpdateStudentStudyYearDto dto)
        {
            Id = id;
            Dto = dto;
        }
    }
}
