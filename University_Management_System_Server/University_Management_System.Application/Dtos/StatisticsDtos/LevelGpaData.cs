using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Application.Dtos.StatisticsDtos
{
    public class LevelGpaData
    {
        public int Level { get; set; }
        public decimal AverageGPA { get; set; }
        public int StudentCount { get; set; }
    }
}