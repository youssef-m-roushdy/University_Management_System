using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Application.Dtos.StatisticsDtos
{
    public class MonthlyEnrollmentData
    {
        public string Month { get; set; } = string.Empty;
        public int Registrations { get; set; }
        public int NewStudents { get; set; }
    }
}