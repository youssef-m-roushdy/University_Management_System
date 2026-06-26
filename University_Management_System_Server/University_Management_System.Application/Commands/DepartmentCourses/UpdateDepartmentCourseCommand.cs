using MediatR;
using University_Management_System.Application.Dtos.DepartmentCourseDtos;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Commands.DepartmentCourses
{
    public class UpdateDepartmentCourseCommand : IRequest<DepartmentCourseDto>
    {
        public int DepartmentId { get; set; }
        public int CourseId { get; set; }
        public UpdateDepartmentCourseDto Dto { get; set; } = null!;
    }
}