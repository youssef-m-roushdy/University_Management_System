using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Application.Dtos.StatisticsDtos
{
    public class GpaDistributionDto
    {
        public List<GpaRangeData> GpaRanges { get; set; } = new();
        public List<LevelGpaData> LevelGpaData { get; set; } = new();
        public decimal AverageGPA { get; set; }
        public int TotalStudents { get; set; }
    }
}