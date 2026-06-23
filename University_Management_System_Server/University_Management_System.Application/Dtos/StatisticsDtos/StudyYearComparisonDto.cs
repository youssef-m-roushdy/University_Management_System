using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Application.Dtos.StatisticsDtos
{
    public class StudyYearComparisonDto
    {
        public StudyYearComparisonData Year1 { get; set; } = new();
        public StudyYearComparisonData Year2 { get; set; } = new();
        public ComparisonSummary Summary { get; set; } = new();
    }
}