using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Application.Dtos.StatisticsDtos
{
    public class SemesterComparisonDto
    {
        public SemesterComparisonData Semester1 { get; set; } = new();
        public SemesterComparisonData Semester2 { get; set; } = new();
        public ComparisonSummary Summary { get; set; } = new();
    }
}