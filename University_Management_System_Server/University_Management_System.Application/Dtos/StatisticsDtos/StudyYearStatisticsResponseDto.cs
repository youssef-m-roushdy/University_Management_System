using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Application.Dtos.StatisticsDtos
{
    public class StudyYearStatisticsResponseDto
    {
        public int StudyYearId { get; set; }
        public string YearRange { get; set; } = string.Empty;
        public bool IsCurrent { get; set; }
        
        // Enrollment Statistics
        public int TotalStudents { get; set; }
        public int TotalRegistrations { get; set; }
        public int TotalCourses { get; set; }
        
        // Staff Statistics
        public int TotalInstructors { get; set; }
        public int TotalAssistants { get; set; }
        
        // Academic Statistics
        public decimal AverageGPA { get; set; }
        public decimal HighestGPA { get; set; }
        public decimal LowestGPA { get; set; }
        public int TotalCreditsEarned { get; set; }
        
        // Financial Statistics
        public int TotalFees { get; set; }
        public decimal TotalFeeAmount { get; set; }
        public decimal AverageFeePerStudent { get; set; }
        
        // Semester Distribution
        public int TotalSemesters { get; set; }
        public int ActiveSemesters { get; set; }
        public int CompletedSemesters { get; set; }
        public int UpcomingSemesters { get; set; }
        
        // Level Distribution
        public Dictionary<string, int> StudentsByLevel { get; set; } = new();
        public Dictionary<string, decimal> AverageGpaByLevel { get; set; } = new();
        
        // Department Distribution
        public Dictionary<string, int> StudentsByDepartment { get; set; } = new();
        public Dictionary<string, int> CoursesByDepartment { get; set; } = new();
        
        // Status
        public string Status { get; set; } = string.Empty;
        public DateTime CalculatedAt { get; set; }
    }
}