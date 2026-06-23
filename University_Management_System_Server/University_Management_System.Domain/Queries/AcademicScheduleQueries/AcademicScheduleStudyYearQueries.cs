using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Domain.Queries.AcademicScheduleQueries
{
    public class AcademicScheduleStudyYearQueries : SearchablePaginationQuery
    {
        // ─── Department Info ──────────────────────────────────────────────────
        public string? DepartmentName { get; set; }
        public string? DepartmentCode { get; set; }
        
        // ─── Schedule Info ────────────────────────────────────────────────────
        public string? Title { get; set; }
        public DateTime? ScheduleDateFrom { get; set; }
        public DateTime? ScheduleDateTo { get; set; }
        
        // ─── Semester Filters ──────────────────────────────────────────────────
        public SemesterEnum? SemesterTitle { get; set; }
    }
}