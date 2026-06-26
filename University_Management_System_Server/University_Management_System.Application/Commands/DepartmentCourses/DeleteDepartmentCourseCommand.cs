using MediatR;

namespace University_Management_System.Application.Commands.DepartmentCourses
{
    public class DeleteDepartmentCourseCommand : IRequest<bool>
    {
        public int DepartmentId { get; set; }
        public int CourseId { get; set; }
    }
}