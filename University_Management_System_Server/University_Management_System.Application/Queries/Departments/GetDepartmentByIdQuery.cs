using University_Management_System.Application.Dtos.DepartmentDtos;
using MediatR;
using University_Management_System.Shared.Respones;

namespace University_Management_System.Application.Queries.Departments
{
    public record GetDepartmentByIdQuery(int Id) : IRequest<Response<DepartmentDto>>;
}
