using MediatR;
using University_Management_System.Application.Dtos.DepartmentCourseDtos;

namespace University_Management_System.Application.Commands.DepartmentCourses
{
    public class CreateDepartmentCourseBulkCommand : IRequest<IEnumerable<DepartmentCourseDto>>
    {
        public CreateDepartmentCourseBulkDto Dto { get; set; } = null!;
    }
}