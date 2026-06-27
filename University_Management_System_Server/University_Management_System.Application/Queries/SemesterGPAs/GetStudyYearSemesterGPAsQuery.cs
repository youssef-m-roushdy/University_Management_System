using MediatR;
using University_Management_System.Application.Dtos.SemesterGPADtos;
using University_Management_System.Domain.Queries.SemesterGPAQueries;

namespace University_Management_System.Application.Queries.SemesterGPAs
{
    public class GetStudyYearSemesterGPAsQuery : IRequest<(IEnumerable<SemesterGPADto> Data, int TotalCount)>
    {
        public int StudyYearId { get; set; }
        public semesterGPAStudyYearQueries Filter { get; set; } = new semesterGPAStudyYearQueries();
    }
}