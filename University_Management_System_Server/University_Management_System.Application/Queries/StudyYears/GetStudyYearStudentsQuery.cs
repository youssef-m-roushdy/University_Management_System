using MediatR;
using University_Management_System.Application.Dtos.StudyYearDtos;
using University_Management_System.Domain.Queries;

namespace University_Management_System.Application.Queries.StudyYears
{
    public class GetStudyYearStudentsQuery : IRequest<(IEnumerable<StudentStudyYearDto> Data, int TotalCount)>
    {    
        public int StudyYearId { get; set; }
        public GetStudyYearNestedQueries Query { get; set; } = new GetStudyYearNestedQueries();
    }
}