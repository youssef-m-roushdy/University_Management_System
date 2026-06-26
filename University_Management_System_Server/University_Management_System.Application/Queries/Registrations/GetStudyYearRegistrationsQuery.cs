using MediatR;
using University_Management_System.Application.Dtos.RegistrationDtos;
using University_Management_System.Domain.Queries.RegistrationQueries;

namespace University_Management_System.Application.Queries.Registrations
{
    public class GetStudyYearRegistrationsQuery : IRequest<(IEnumerable<RegistrationDto> Data, int TotalCount)>
    {
        public int StudyYearId { get; set; }
        public RegistrationStudyYearQueries? Query { get; set; }
    }
}