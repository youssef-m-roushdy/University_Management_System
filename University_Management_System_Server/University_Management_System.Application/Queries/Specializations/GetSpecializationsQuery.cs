using MediatR;
using University_Management_System.Application.Dtos.SpecializationDtos;
using University_Management_System.Domain.Queries.SpecializationQueries;

namespace University_Management_System.Application.Queries.Specializations
{
    public class GetSpecializationsQuery : IRequest<(IEnumerable<SpecializationDto> Data, int TotalCount)>
    {
        public SpecializationFilterQueries Query { get; set; } = new SpecializationFilterQueries();
    }
}