using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Application.Dtos.StatisticsDtos
{
    public class ComparisonSummary
    {
        public string StudentChange { get; set; } = string.Empty;
        public string GpaChange { get; set; } = string.Empty;
        public string PassRateChange { get; set; } = string.Empty;
        public string RegistrationChange { get; set; } = string.Empty;
        public bool IsImprovement { get; set; }
    }
}