using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Application.Dtos.StatisticsDtos
{
    public class StudyYearComparisonData
    {
        public int StudyYearId { get; set; }
        public string YearRange { get; set; } = string.Empty;
        public int TotalStudents { get; set; }
        public decimal AverageGPA { get; set; }
        public decimal PassRate { get; set; }
        public int TotalCourses { get; set; }
        public int TotalRegistrations { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}