using MediatR;
using University_Management_System.Application.Dtos.StudyYearDtos;

namespace University_Management_System.Application.Queries.StudyYears
{
    public class GetStudyYearQuery : IRequest<StudyYearDto>
    {
        public int Id { get; set; }
    }
}