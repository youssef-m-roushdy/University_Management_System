using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Application.Dtos.StatisticsDtos
{
    public class DepartmentStatisticsDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        
        public int TotalStudents { get; set; }
        public int TotalInstructors { get; set; }
        public int TotalAssistants { get; set; }
        public int TotalCourses { get; set; }
        
        public decimal AverageGPA { get; set; }
        public decimal PassRate { get; set; }
        public int GraduatedStudents { get; set; }
        public int CurrentStudents { get; set; }
        
        public Dictionary<int, int> StudentsByLevel { get; set; } = new();
        public Dictionary<int, decimal> AverageGpaByLevel { get; set; } = new();
        public Dictionary<string, int> StudentsBySpecialization { get; set; } = new();
    }
}