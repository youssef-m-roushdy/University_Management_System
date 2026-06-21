using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Domain.Queries
{
    public class FeeQueries : PaginationQuery
    {
        public int? StudyYearId { get; set; }
        public int? DepartmentId { get; set; }
        public int? Level { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
    }
}