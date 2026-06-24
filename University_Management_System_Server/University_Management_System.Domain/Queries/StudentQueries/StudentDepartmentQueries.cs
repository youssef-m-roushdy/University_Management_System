using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Domain.Queries.StudentQueries
{
    public class StudentDepartmentQueries : SearchablePaginationQuery
    {
        public Levels? Level { get; set; }
        public Gender? Gender { get; set; }
        public string? SpecializationSearch { get; set; } // Name only
        public decimal? MinGPA { get; set; }
        public decimal? MaxGPA { get; set; }
        public int? MinTotalCredits { get; set; }
        public int? MaxTotalCredits { get; set; }
        public int? MinAllowedCredits { get; set; }
        public int? MaxAllowedCredits { get; set; }
        public bool? IsGraduated { get; set; }
        public bool? IsActive { get; set; }
        // by default searchTerm inherited from SearchablePaginationQuery will search about student name academic code email
    }
}