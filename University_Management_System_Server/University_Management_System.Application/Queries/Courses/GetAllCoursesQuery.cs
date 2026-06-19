using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.CourseDtos;
using University_Management_System.Domain.Queries;
using MediatR;
using University_Management_System.Shared.Responses;

namespace University_Management_System.Application.Queries.Courses
{
    public class GetAllCoursesQuery : IRequest<PagedResponse<CourseWithDepartmentDto>>
    {
        public CourseQuery Query { get; set; }

        public GetAllCoursesQuery(CourseQuery query)
        {
            Query = query;
        }
    }
}