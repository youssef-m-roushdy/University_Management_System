using MediatR;
using University_Management_System.Application.Dtos.DepartmentCourseDtos;
using University_Management_System.Domain.Queries.DepartmentCourseQueries;

namespace University_Management_System.Application.Queries.DepartmentCourses
{
    public class GetAllDepartmentCoursesQuery : IRequest<(IEnumerable<DepartmentCourseDto> Data, int TotalCount)>
    {
        public CourseFilterInDepartmentQueries Query { get; set; } = new CourseFilterInDepartmentQueries();
    }
}