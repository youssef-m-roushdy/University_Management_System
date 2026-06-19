using University_Management_System.Application.Dtos.StudentStudyYearDtos;
using MediatR;
using University_Management_System.Shared.Responses;

namespace University_Management_System.Application.Commands.StudentStudyYears
{
    public class CreateStudentStudyYearCommand : IRequest<ApiResponse<StudentStudyYearDto>>
    {
        public CreateStudentStudyYearDto Dto { get; set; } = null!;

        public CreateStudentStudyYearCommand(CreateStudentStudyYearDto dto)
        {
            Dto = dto;
        }
    }
}
