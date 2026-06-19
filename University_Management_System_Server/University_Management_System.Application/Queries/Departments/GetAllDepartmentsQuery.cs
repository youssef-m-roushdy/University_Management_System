using University_Management_System.Application.Dtos.DepartmentDtos;
using MediatR;
using University_Management_System.Shared.Responses;

namespace University_Management_System.Application.Queries.Departments
{
    public record GetAllDepartmentsQuery : IRequest<ApiResponse<IEnumerable<DepartmentDto>>>;
}
