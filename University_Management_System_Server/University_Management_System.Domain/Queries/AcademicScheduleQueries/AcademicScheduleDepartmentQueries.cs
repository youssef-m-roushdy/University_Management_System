using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Domain.Queries.AcademicScheduleQueries
{
    public class AcademicScheduleDepartmentQueries : SearchablePaginationQuery
    {
        // ─── Schedule Info ────────────────────────────────────────────────────
        public string? Title { get; set; }
        public DateTime? ScheduleDateFrom { get; set; }
        public DateTime? ScheduleDateTo { get; set; }
        
        // ─── Study Year Filters ──────────────────────────────────────────────
        public int? StudyYearStart { get; set; }
        public int? StudyYearEnd { get; set; }
        // ─── Semester Filters ──────────────────────────────────────────────────
        public SemesterEnum? SemesterTitle { get; set; } 
    }
}