using MediatR;
using University_Management_System.Application.Dtos.StudyYearDtos;
using University_Management_System.Domain.Queries;
using University_Management_System.Domain.Queries.StudyYearQueries;

namespace University_Management_System.Application.Queries.StudyYears
{
    public class GetStudyYearsQuery : IRequest<(IEnumerable<StudyYearDto> Data, int TotalCount)>
    {
        public StudyYearFilterQueries Query { get; set; } = new StudyYearFilterQueries();
    }
}