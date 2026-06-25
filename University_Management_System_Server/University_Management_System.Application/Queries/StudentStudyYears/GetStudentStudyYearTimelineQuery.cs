using University_Management_System.Application.Dtos.StudentStudyYearDtos;
using MediatR;
using University_Management_System.Shared.Responses;

namespace University_Management_System.Application.Queries.StudentStudyYears
{
    public class GetStudentStudyYearTimelineQuery : IRequest<StudentStudyYearTimelineDto>
    {
        public string StudentId { get; set; } = string.Empty;

    }
}
