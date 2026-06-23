using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Application.Dtos.StatisticsDtos
{
    public class CourseEnrollmentData
    {
        public string CourseName { get; set; } = string.Empty;
        public int EnrolledStudents { get; set; }
    }
}