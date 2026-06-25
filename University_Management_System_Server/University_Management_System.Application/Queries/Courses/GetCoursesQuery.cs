using MediatR;
using University_Management_System.Application.Dtos.CourseDtos;
using University_Management_System.Domain.Queries.CourseQueries;

namespace University_Management_System.Application.Queries.Courses
{
    public class GetCoursesQuery : IRequest<(IEnumerable<CourseDto> Data, int TotalCount)>
    {
        public CourseFilterQueries Query { get; set; } = new CourseFilterQueries();
    }
}