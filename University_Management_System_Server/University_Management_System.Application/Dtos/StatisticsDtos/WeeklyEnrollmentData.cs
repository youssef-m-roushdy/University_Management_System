using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Application.Dtos.StatisticsDtos
{
    public class WeeklyEnrollmentData
    {
        public string Week { get; set; } = string.Empty;
        public int Registrations { get; set; }
    }
}