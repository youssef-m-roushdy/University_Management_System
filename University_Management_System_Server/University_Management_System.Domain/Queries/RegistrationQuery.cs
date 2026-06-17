using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Domain.Queries
{
    public class RegistrationQuery : PaginationQuery
    {
        public string? StudentUserName { get; set; }
        public string? CourseName { get; set; }
        public string? AcademicCode { get; set; }
        public string? CourseCode { get; set; }
        public RegistrationStatus? Status { get; set; }
        public bool? IsPassed { get; set; }
        public CourseProgress? Progress { get; set; }
        public Grades? Grade { get; set; } 
    }
}