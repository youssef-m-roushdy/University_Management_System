using MediatR;
using University_Management_System.Application.Dtos.SpecializationCourseDtos;

namespace University_Management_System.Application.Commands.SpecializationCourses
{
    public class CreateSpecializationCourseBulkCommand : IRequest<IEnumerable<SpecializationCourseDto>>
    {
        public CreateSpecializationCourseBulkDto Dto { get; set; } = null!;
    }
}