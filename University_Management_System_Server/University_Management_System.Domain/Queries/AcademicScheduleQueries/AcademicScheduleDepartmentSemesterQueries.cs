using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Domain.Queries.AcademicScheduleQueries
{
    public class AcademicScheduleDepartmentSemesterQueries : SearchablePaginationQuery
    {
        
        // ─── Schedule Info ────────────────────────────────────────────────────
        public string? Title { get; set; }
        public DateTime? ScheduleDateFrom { get; set; }
        public DateTime? ScheduleDateTo { get; set; }
        
        // ─── Study Year Filters ──────────────────────────────────────────────
        public int? StudyYearStart { get; set; }
        public int? StudyYearEnd { get; set; }
    }
}