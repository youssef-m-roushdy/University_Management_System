using MediatR;
using University_Management_System.Application.Dtos.RegistrationDtos;

namespace University_Management_System.Application.Queries.Registrations
{
    public class GetRegisteredCoursesQuery : IRequest<IEnumerable<RegistrationDto>>
    {
        public string StudentId { get; set; } = string.Empty;
    }
}