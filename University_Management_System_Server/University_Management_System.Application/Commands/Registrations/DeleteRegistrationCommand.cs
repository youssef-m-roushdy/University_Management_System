using MediatR;

namespace University_Management_System.Application.Commands.Registrations
{
    public class DeleteRegistrationCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
    }
}