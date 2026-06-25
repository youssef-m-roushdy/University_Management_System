using MediatR;
using University_Management_System.Application.Dtos.CourseDtos;

namespace University_Management_System.Application.Queries.Courses
{
    public class GetCourseDependenciesQuery : IRequest<IEnumerable<CourseDto>>
    {
        public int CourseId { get; set; }
    }
}