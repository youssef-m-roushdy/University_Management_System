using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Application.Dtos.StatisticsDtos
{
    public class SemesterStatisticsResponseDto
    {
        public int SemesterId { get; set; }
        public string SemesterTitle { get; set; } = string.Empty;
        public string YearRange { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; } = string.Empty;
        
        // Enrollment Statistics
        public int TotalStudents { get; set; }
        public int TotalRegistrations { get; set; }
        public int TotalCourses { get; set; }
        public double AverageCoursesPerStudent { get; set; }
        
        // Academic Statistics
        public decimal SemesterAverageGPA { get; set; }
        public decimal HighestGPA { get; set; }
        public decimal LowestGPA { get; set; }
        public int TotalCreditsEarned { get; set; }
        public decimal PassRate { get; set; }
        public decimal FailRate { get; set; }
        
        // GPA Distribution
        public Dictionary<string, int> GpaDistribution { get; set; } = new();
        public Dictionary<string, int> GradeDistribution { get; set; } = new();
        
        // Course Statistics
        public Dictionary<string, int> PopularCourses { get; set; } = new();
        public Dictionary<string, decimal> CourseAverageGpa { get; set; } = new();
        
        // Student Performance
        public int StudentsOnProbation { get; set; }
        public int StudentsWithHonors { get; set; }
        public decimal TopStudentGPA { get; set; }
        
        // Department Performance
        public Dictionary<string, decimal> DepartmentAverageGpa { get; set; } = new();
        
        // Daily Stats
        public Dictionary<string, int> DailyRegistrations { get; set; } = new();
        
        public DateTime CalculatedAt { get; set; }
    }
}