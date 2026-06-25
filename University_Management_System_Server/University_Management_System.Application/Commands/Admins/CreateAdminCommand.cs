using MediatR;
using University_Management_System.Application.Dtos.AdminDtos;

namespace University_Management_System.Application.Commands.Admins
{
    public class CreateAdminCommand : IRequest<AdminDto>
    {
        public CreateAdminDto Dto { get; set; } = null!;
    }
}