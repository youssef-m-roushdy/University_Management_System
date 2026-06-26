using MediatR;
using University_Management_System.Application.Dtos.SpecializationDtos;
using University_Management_System.Domain.Queries.SpecializationQueries;

namespace University_Management_System.Application.Queries.Specializations
{
    public class GetSpecializationsByDepartmentQuery : IRequest<(IEnumerable<SpecializationDto> Data, int TotalCount)>
    {
        public int DepartmentId { get; set; }
        public SpecializationDepartmentQueries Query { get; set; } = new SpecializationDepartmentQueries();
    }
}