using MediatR;
using University_Management_System.Application.Dtos.CourseDtos;

namespace University_Management_System.Application.Queries.Courses
{
    public class GetCourseQuery : IRequest<CourseDto?>
    {
        public int Id { get; set; }
    }
}