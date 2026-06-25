using MediatR;
using University_Management_System.Application.Dtos.CourseDtos;
using University_Management_System.Domain.Queries.CourseQueries;

namespace University_Management_System.Application.Queries.Courses
{
    public class GetDepartmentCoursesQuery : IRequest<(IEnumerable<CourseDto> Data, int TotalCount)>
    {
        public int DepartmentId { get; set; }
        public CourseDepartmentQueries Query { get; set; } = new CourseDepartmentQueries();
    }
}