using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Application.Dtos.StatisticsDtos
{
    public class SemesterOverviewDto
    {
        public int SemesterId { get; set; }
        public string SemesterTitle { get; set; } = string.Empty;
        public string YearRange { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        
        public int TotalStudents { get; set; }
        public int TotalCourses { get; set; }
        public decimal AverageGPA { get; set; }
        public decimal PassRate { get; set; }
        
        public int NewStudents { get; set; }
        public int GraduatingStudents { get; set; }
        public int DaysRemaining { get; set; }
    }
}