using MediatR;
using University_Management_System.Application.Dtos.DepartmentCourseDtos;

namespace University_Management_System.Application.Commands.DepartmentCourses
{
    public class CreateDepartmentCourseCommand : IRequest<DepartmentCourseDto>
    {
        public CreateDepartmentCourseDto Dto { get; set; } = null!;
    }
}