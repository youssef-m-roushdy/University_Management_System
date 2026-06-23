using MediatR;

namespace University_Management_System.Application.Commands.Semesters
{
    public class DeleteSemesterCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}