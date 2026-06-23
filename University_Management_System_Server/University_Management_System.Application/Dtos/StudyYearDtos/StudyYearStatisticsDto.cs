using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Application.Dtos.StudyYearDtos
{
    public class StudyYearStatisticsDto
    {
        public int TotalSemesters { get; set; }
        public int TotalRegistrations { get; set; }
        public int TotalStudents { get; set; }
        public int TotalFees { get; set; }
        public decimal TotalFeeAmount { get; set; }
        public int TotalCourses { get; set; }
        public int TotalInstructors { get; set; }
        public int TotalAssistants { get; set; }
        public int DurationInYears { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}