using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.CourseDtos;
using MediatR;

namespace University_Management_System.Application.Queries.Courses
{
    public class GetDepartmentOpenCoursesQuery : IRequest<IEnumerable<CourseDto>>
    {
        public int DepartmentId {get; set;}

        public GetDepartmentOpenCoursesQuery(int departmentId)
        {
            DepartmentId = departmentId;
        }
    }
}