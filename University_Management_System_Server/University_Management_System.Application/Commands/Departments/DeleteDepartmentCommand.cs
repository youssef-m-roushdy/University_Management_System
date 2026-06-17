using MediatR;

namespace University_Management_System.Application.Commands.Departments
{
    public class DeleteDepartmentCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public DeleteDepartmentCommand(int id)
        {
            Id = id;
        }
    }
}