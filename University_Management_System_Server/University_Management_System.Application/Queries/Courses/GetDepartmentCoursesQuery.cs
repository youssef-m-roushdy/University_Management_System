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
    public class GetDepartmentCoursesQuery : IRequest<PagedResponse<DepartmentCourseDto>>
    {
        public int DepartmentId { get; set; }
        public DepartmentCourseQuery Query { get; set; }

        public GetDepartmentCoursesQuery(int departmentId, DepartmentCourseQuery query)       
        {
            Query = query;
            DepartmentId = departmentId;
        }
    }
}