using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Application.Dtos.StatisticsDtos
{
    public class StudyYearOverviewDto
    {
        public int StudyYearId { get; set; }
        public string YearRange { get; set; } = string.Empty;
        public bool IsCurrent { get; set; }
        public string Status { get; set; } = string.Empty;
        
        // Key Metrics
        public int TotalStudents { get; set; }
        public int TotalCourses { get; set; }
        public decimal OverallAverageGPA { get; set; }
        public decimal PassRate { get; set; }
        
        // Quick Stats
        public int NewStudents { get; set; }
        public int GraduatingStudents { get; set; }
        public int ActiveSemesters { get; set; }
        
        // Financial
        public decimal TotalRevenue { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal NetRevenue { get; set; }
    }
}