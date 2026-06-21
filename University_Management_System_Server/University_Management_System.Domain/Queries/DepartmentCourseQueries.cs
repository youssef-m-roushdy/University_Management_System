using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Domain.Queries
{
    public class DepartmentCourseQueries : PaginationQuery
    {
        public CourseStatus? Status { get; set; }
    }
}