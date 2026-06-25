using MediatR;
using University_Management_System.Application.Dtos.CourseDtos;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Commands.Courses
{
    public class UpdateCourseStatusCommand : IRequest<CourseDto>
    {
        public int Id { get; set; }
        public CourseStatus Status { get; set; }
    }
}