using MediatR;

namespace University_Management_System.Application.Commands.Admins
{
    public class DeleteAdminCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
    }
}