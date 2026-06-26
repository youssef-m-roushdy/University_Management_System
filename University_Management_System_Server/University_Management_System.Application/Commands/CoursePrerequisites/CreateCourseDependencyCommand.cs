using MediatR;
using University_Management_System.Application.Dtos.CourseDtos;
using University_Management_System.Application.Dtos.CoursePrerequisiteDtos;

namespace University_Management_System.Application.Commands.CoursePrerequisites
{
    public class CreateCourseDependencyCommand : IRequest<CourseDto>
    {
        public CreateCourseDependencyDto Dto { get; set; } = null!;
    }
}