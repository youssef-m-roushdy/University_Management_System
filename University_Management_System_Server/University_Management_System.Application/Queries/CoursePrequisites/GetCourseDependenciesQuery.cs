using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.CourseDtos;
using MediatR;

namespace University_Management_System.Application.Queries.CoursePrequisites
{
    public class GetCourseDependenciesQuery : IRequest<List<CourseDto>>
    {
        public int CourseId { get; set; }
        public GetCourseDependenciesQuery(int courseId)
        {
            CourseId = courseId;
        }
    }
}