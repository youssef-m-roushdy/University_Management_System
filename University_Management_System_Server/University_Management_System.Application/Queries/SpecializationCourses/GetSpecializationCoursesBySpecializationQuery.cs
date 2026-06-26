using MediatR;
using University_Management_System.Application.Dtos.SpecializationCourseDtos;
using University_Management_System.Domain.Queries.SpecializationCourseQueries;

namespace University_Management_System.Application.Queries.SpecializationCourses
{
    public class GetSpecializationCoursesBySpecializationQuery : IRequest<(IEnumerable<SpecializationCourseDto> Data, int TotalCount)>
    {
        public int SpecializationId { get; set; }
        public CourseFilterInSpecailizationQueries Query { get; set; } = new CourseFilterInSpecailizationQueries();
    }
}