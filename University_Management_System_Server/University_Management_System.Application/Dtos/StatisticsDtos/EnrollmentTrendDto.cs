using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Application.Dtos.StatisticsDtos
{
    public class EnrollmentTrendDto
    {
        public List<MonthlyEnrollmentData> MonthlyData { get; set; } = new();
        public List<WeeklyEnrollmentData> WeeklyData { get; set; } = new();
        public List<CourseEnrollmentData> CourseEnrollment { get; set; } = new();
    }
}