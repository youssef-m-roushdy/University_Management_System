using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Domain.Queries.RegistrationQueries
{
    public class RegistrationStudyYearQueries : SearchablePaginationQuery
    {
        // ─── Student Info ─────────────────────────────────────────────────────
        public string? StudentName { get; set; }
        public string? AcademicCode { get; set; }
        
        // ─── Course Info ──────────────────────────────────────────────────────
        public string? CourseName { get; set; }
        public string? CourseCode { get; set; }
        
        // ─── Registration Status ─────────────────────────────────────────────
        public RegistrationStatus? Status { get; set; }
        public bool? IsPassed { get; set; }
        public CourseProgress? Progress { get; set; }
        public Grades? Grade { get; set; }
        
        // ─── Date Filters ─────────────────────────────────────────────────────
        public DateTime? RegisteredFrom { get; set; }
        public DateTime? RegisteredTo { get; set; }

        // ─── Semester Filters ──────────────────────────────────────────────────
        public SemesterEnum? SemesterTitle { get; set; }
    }
}