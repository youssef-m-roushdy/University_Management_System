using MediatR;
using University_Management_System.Application.Dtos.AdminDtos;
using University_Management_System.Domain.Queries.AdminQueries;

namespace University_Management_System.Application.Queries.Admins
{
    public class GetAdminsQuery : IRequest<(IEnumerable<AdminDto> Data, int TotalCount)>
    {
        public AdminFilterQueries Query { get; set; } = new AdminFilterQueries();
    }
}