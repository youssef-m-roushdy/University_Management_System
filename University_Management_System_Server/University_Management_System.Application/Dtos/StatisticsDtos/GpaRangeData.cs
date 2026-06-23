using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Application.Dtos.StatisticsDtos
{
    public class GpaRangeData
    {
        public string Range { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal Percentage { get; set; }
    }
}