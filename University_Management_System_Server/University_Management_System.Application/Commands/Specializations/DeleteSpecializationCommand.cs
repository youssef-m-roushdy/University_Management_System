using MediatR;

namespace University_Management_System.Application.Commands.Specializations
{
    public class DeleteSpecializationCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}