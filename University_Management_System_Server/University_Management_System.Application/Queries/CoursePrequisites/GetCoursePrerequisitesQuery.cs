using MediatR;
using University_Management_System.Application.Dtos.CourseDtos;

namespace University_Management_System.Application.Queries.CoursePrequisites
{
    public class GetCoursePrerequisitesQuery : IRequest<IEnumerable<CourseDto>>
    {
        public int CourseId { get; set; }
    }
}