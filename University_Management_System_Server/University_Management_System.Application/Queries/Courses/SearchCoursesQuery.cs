using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using University_Management_System.Application.Dtos.CourseDtos;

namespace University_Management_System.Application.Queries.Courses
{
    public class SearchCoursesQuery : IRequest<IEnumerable<CourseSearchResultDto?>>
    {
        public string? SearchTerm { get; set; }
        public int? DepartmentId { get; set; }
        public int? MaxResults { get; set; } = 20;
    }
}