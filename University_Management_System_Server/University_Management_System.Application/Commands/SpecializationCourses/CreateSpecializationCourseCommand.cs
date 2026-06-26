using MediatR;
using University_Management_System.Application.Dtos.SpecializationCourseDtos;

namespace University_Management_System.Application.Commands.SpecializationCourses
{
    public class CreateSpecializationCourseCommand : IRequest<SpecializationCourseDto>
    {
        public CreateSpecializationCourseDto Dto { get; set; } = null!;
    }
}