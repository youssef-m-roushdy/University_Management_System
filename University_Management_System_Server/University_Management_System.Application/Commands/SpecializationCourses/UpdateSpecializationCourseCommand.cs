using MediatR;
using University_Management_System.Application.Dtos.SpecializationCourseDtos;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Commands.SpecializationCourses
{
    public class UpdateSpecializationCourseCommand : IRequest<SpecializationCourseDto>
    {
        public int SpecializationId { get; set; }
        public int CourseId { get; set; }
        public UpdateSpecializationCourseDto Dto { get; set; } = null!;
    }
}