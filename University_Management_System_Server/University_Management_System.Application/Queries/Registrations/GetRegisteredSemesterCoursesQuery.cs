using MediatR;
using University_Management_System.Application.Dtos.RegistrationDtos;

namespace University_Management_System.Application.Queries.Registrations
{
    public class GetRegisteredSemesterCoursesQuery : IRequest<IEnumerable<RegistrationDto>>
    {
        public int StudyYearId { get; set; }
        public int SemesterId { get; set; }
        public string StudentId { get; set; } = string.Empty;
    }
}