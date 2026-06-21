using MediatR;

namespace University_Management_System.Application.Commands.StudyYears
{
    public class DeleteStudyYearCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}