using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Application.Dtos.StatisticsDtos
{
    public class DepartmentEnrollmentData
    {
        public string DepartmentName { get; set; } = string.Empty;
        public int StudentCount { get; set; }
        public int CourseCount { get; set; }
        public int InstructorCount { get; set; }
        public decimal Percentage { get; set; }
    }
}