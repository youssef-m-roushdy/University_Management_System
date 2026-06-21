using MediatR;
using University_Management_System.Application.Dtos.StudyYearDtos;
using University_Management_System.Domain.Queries;

namespace University_Management_System.Application.Queries.StudyYears
{
    public class GetStudyYearsQuery : IRequest<(IEnumerable<StudyYearDto> Data, int TotalCount)>
    {
        public StudyYearQueries Query { get; set; } = new StudyYearQueries();
    }
}