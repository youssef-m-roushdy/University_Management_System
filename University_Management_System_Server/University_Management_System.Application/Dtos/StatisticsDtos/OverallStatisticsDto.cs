using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Application.Dtos.StatisticsDtos
{
    public class OverallStatisticsDto
    {
        // Institution Overview
        public int TotalUsers { get; set; }
        public int TotalStudents { get; set; }
        public int TotalInstructors { get; set; }
        public int TotalAssistants { get; set; }
        public int TotalAdmins { get; set; }
        
        // Academic Overview
        public int TotalStudyYears { get; set; }
        public int TotalSemesters { get; set; }
        public int TotalCourses { get; set; }
        public int TotalDepartments { get; set; }
        public int TotalSpecializations { get; set; }
        
        // Current Status
        public int CurrentStudyYearId { get; set; }
        public string CurrentStudyYear { get; set; } = string.Empty;
        public int CurrentSemesterId { get; set; }
        public string CurrentSemester { get; set; } = string.Empty;
        
        // Overall GPA
        public decimal OverallAverageGPA { get; set; }
        public decimal OverallPassRate { get; set; }
        
        // Financial Overview
        public decimal TotalRevenue { get; set; }
        public decimal TotalFeesCollected { get; set; }
        
        // Active Users
        public int ActiveStudents { get; set; }
        public int InactiveStudents { get; set; }
        public int ActiveInstructors { get; set; }
        
        public DateTime CalculatedAt { get; set; }
    }
}