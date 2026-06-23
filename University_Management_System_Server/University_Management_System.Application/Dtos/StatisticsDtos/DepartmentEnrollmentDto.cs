using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Application.Dtos.StatisticsDtos
{
    public class DepartmentEnrollmentDto
    {
        public List<DepartmentEnrollmentData> Departments { get; set; } = new();
        public int TotalStudents { get; set; }
    }
}