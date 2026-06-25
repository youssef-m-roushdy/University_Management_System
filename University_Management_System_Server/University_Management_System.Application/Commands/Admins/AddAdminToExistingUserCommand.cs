using MediatR;
using University_Management_System.Application.Dtos.AdminDtos;

namespace University_Management_System.Application.Commands.Admins
{
    public class AddAdminToExistingUserCommand : IRequest<AdminDto>
    {
        public AddAdminToExistingUserDto Dto { get; set; } = null!;
    }
}