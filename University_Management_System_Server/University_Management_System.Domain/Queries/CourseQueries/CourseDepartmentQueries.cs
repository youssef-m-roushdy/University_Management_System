using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Domain.Queries.CourseQueries
{
    public class CourseDepartmentQueries : SearchablePaginationQuery
    {
        public CourseStatus? Status { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? MinCredits { get; set; }
        public int? MaxCredits { get; set; }
        public bool? HasPrerequisites { get; set; }
    }
}