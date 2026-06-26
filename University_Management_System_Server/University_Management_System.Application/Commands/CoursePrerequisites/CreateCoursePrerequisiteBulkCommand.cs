using MediatR;
using University_Management_System.Application.Dtos.CourseDtos;
using University_Management_System.Application.Dtos.CoursePrerequisiteDtos;

namespace University_Management_System.Application.Commands.CoursePrerequisites
{
    public class CreateCoursePrerequisiteBulkCommand : IRequest<IEnumerable<CourseDto>>
    {
        public CreateCoursePrerequisiteBulkDto Dto { get; set; } = null!;
    }
}