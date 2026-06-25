using MediatR;
using University_Management_System.Application.Dtos.CourseDtos;

namespace University_Management_System.Application.Commands.Courses
{
    public class CreateCourseCommand : IRequest<CourseDto>
    {
        public CreateCourseDto Dto { get; set; } = null!;
    }
}