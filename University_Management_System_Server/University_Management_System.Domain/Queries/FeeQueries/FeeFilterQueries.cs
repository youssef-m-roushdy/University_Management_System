using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Domain.Queries.FeeQueries
{
    public class FeeFilterQueries : PaginationQuery
    {
        // inherited from PaginationQuery no searchPaginationQuery becase fees is based on number of records not search term
        public string? DepartmentName { get; set; }
        public string? DepartmentCode { get; set; }
        public Levels? Level { get; set; }
        public FeeType? FeeType { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
         // ─── Study Year Filters ─────────────────────────────────────────────────
        public int? StudyYearStart { get; set; }
        public int? StudyYearEnd { get; set; }
    }
}