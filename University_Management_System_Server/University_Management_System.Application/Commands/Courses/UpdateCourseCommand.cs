using MediatR;
using University_Management_System.Application.Dtos.CourseDtos;

namespace University_Management_System.Application.Commands.Courses
{
    public class UpdateCourseCommand : IRequest<CourseDto>
    {
        public int Id { get; set; }
        public UpdateCourseDto Dto { get; set; } = null!;
    }
}