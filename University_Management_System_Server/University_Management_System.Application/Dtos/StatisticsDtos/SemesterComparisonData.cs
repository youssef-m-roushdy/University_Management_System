using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Application.Dtos.StatisticsDtos
{
    public class SemesterComparisonData
    {
        public int SemesterId { get; set; }
        public string SemesterTitle { get; set; } = string.Empty;
        public string YearRange { get; set; } = string.Empty;
        public int TotalStudents { get; set; }
        public decimal AverageGPA { get; set; }
        public decimal PassRate { get; set; }
        public int TotalCourses { get; set; }
        public int TotalRegistrations { get; set; }
    }
}