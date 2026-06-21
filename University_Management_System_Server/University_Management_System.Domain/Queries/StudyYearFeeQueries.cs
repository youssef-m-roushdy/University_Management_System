using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Domain.Queries
{
    public class StudyYearFeeQueries : PaginationQuery
    {
        // no need for study year id i will take from path parameter in the controller
        public int? DepartmentId { get; set; }
        public Levels? Level { get; set; }
        public FeeType? FeeType { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
    }
}