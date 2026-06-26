using MediatR;
using University_Management_System.Application.Dtos.RegistrationDtos;
using University_Management_System.Domain.Queries.RegistrationQueries;

namespace University_Management_System.Application.Queries.Registrations
{
    public class GetStudentAllRegistrationsQuery : IRequest<(IEnumerable<RegistrationDto> Data, int TotalCount)>
    {
        public string StudentId { get; set; } = string.Empty;
        public RegistrationFilterQueries? Filter { get; set; }
    }
}