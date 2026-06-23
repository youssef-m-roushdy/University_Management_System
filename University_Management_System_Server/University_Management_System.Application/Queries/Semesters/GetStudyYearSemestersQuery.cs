using MediatR;
using University_Management_System.Application.Dtos.SemesterDtos;
using University_Management_System.Application.Dtos.StudyYearDtos;
using University_Management_System.Domain.Queries;

namespace University_Management_System.Application.Queries.Semesters
{
    public class GetStudyYearSemestersQuery : IRequest<(IEnumerable<SemesterDto> Data, int TotalCount)>
    {
        public int StudyYearId { get; set; }
    }
}