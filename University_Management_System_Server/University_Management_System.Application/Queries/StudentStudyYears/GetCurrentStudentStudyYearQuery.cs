using University_Management_System.Application.Dtos.StudentStudyYearDtos;
using MediatR;
using University_Management_System.Shared.Responses;

namespace University_Management_System.Application.Queries.StudentStudyYears
{
    public class GetCurrentStudentStudyYearQuery : IRequest<ApiResponse<StudentStudyYearDto>>
    {
        public string StudentId { get; set; } = string.Empty;

        public GetCurrentStudentStudyYearQuery(string StudentId)
        {
            StudentId = StudentId;
        }
    }
}
