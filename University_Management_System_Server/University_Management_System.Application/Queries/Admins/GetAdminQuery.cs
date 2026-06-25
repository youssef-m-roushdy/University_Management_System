using MediatR;
using University_Management_System.Application.Dtos.AdminDtos;

namespace University_Management_System.Application.Queries.Admins
{
    public class GetAdminQuery : IRequest<AdminDto?>
    {
        public string Id { get; set; } = string.Empty;
    }
}