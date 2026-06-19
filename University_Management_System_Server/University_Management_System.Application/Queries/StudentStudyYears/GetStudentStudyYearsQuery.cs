using University_Management_System.Application.Dtos.StudentStudyYearDtos;
using MediatR;
using University_Management_System.Shared.Responses;

namespace University_Management_System.Application.Queries.StudentStudyYears
{
    public class GetStudentStudyYearsQuery : IRequest<ApiResponse<List<StudentStudyYearDto>>>
    {
        public string StudentId { get; set; } = string.Empty;

        public GetStudentStudyYearsQuery(string StudentId)
        {
            StudentId = StudentId;
        }
    }
}
