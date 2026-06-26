using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Domain.Queries.DepartmentCourseQueries
{
    public class CourseFilterInDepartmentQueries
    {
        public CourseRole? Role { get; set; }
        public string? CourseName { get; set; }
        public string? CourseCode { get; set; }
        public int? MinCredits { get; set; }
        public int? MaxCredits { get; set; }
    }
}