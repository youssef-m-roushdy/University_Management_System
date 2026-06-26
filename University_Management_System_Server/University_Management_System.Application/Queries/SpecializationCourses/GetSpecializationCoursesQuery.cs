using MediatR;
using University_Management_System.Application.Dtos.SpecializationCourseDtos;
using University_Management_System.Domain.Queries.SpecializationCourseQueries;

namespace University_Management_System.Application.Queries.SpecializationCourses
{
    public class GetSpecializationCoursesQuery : IRequest<(IEnumerable<SpecializationCourseDto> Data, int TotalCount)>
    {
        public SpecializationCourseFilterQueries Query { get; set; } = new SpecializationCourseFilterQueries();
    }
}