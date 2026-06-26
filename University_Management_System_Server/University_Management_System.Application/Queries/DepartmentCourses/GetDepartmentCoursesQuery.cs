using MediatR;
using University_Management_System.Application.Dtos.DepartmentCourseDtos;
using University_Management_System.Domain.Queries.DepartmentCourseQueries;

namespace University_Management_System.Application.Queries.DepartmentCourses
{
    public class GetDepartmentCoursesQuery : IRequest<(IEnumerable<DepartmentCourseDto> Data, int TotalCount)>
    {
        public int DepartmentId { get; set; }
        public DepartmentCourseFilterQueries Query { get; set; } = new DepartmentCourseFilterQueries();
    }
}