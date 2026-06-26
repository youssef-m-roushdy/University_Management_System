using MediatR;
using University_Management_System.Application.Dtos.SpecializationDtos;

namespace University_Management_System.Application.Queries.Specializations
{
    public class GetSpecializationQuery : IRequest<SpecializationDto?>
    {
        public int Id { get; set; }
    }
}