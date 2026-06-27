using MediatR;

namespace University_Management_System.Application.Commands.AcademicSchedules
{
    public class DeleteAcademicScheduleCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string AdminId { get; set; } = string.Empty;
    }
}