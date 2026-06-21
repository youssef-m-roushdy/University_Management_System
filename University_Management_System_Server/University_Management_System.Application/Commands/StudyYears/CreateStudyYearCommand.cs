using MediatR;
using University_Management_System.Application.Dtos.StudyYearDtos;

namespace University_Management_System.Application.Commands.StudyYears
{
    public class CreateStudyYearCommand : IRequest<StudyYearDto>
    {
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public bool IsCurrent { get; set; }
    }
}