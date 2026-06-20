using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Domain.Queries
{
    public class StudentQueries : PaginationQuery
    {
        // Student-specific filters
        public string? AcademicCode { get; set; }
        public string? Name { get; set; }
        public Gender? Gender { get; set; }
        public Levels? Level { get; set; }
        public int? DepartmentId { get; set; }
        public int? SpecializationId { get; set; }
        public decimal? MinGPA { get; set; }
        public decimal? MaxGPA { get; set; }
        public int? MinCredits { get; set; }
        public int? MaxCredits { get; set; }
        public bool? IsGraduated { get; set; }
        public bool? IsActive { get; set; }
    }
}