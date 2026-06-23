using MediatR;
using University_Management_System.Application.Dtos.StudyYearDtos;

namespace University_Management_System.Application.Commands.StudyYears
{
    public class PatchStudyYearCommand : IRequest<StudyYearDto>
    {
        public int Id { get; set; }
        public PatchStudyYearDto Dto { get; set; }
    }
}