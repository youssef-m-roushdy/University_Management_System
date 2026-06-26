using MediatR;
using University_Management_System.Application.Dtos.SpecializationCourseDtos;

namespace University_Management_System.Application.Queries.SpecializationCourses
{
    public class GetSpecializationCoursesByCourseQuery : IRequest<IEnumerable<SpecializationCourseDto>>
    {
        public int CourseId { get; set; }
    }
}