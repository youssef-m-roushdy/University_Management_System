using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Domain.Queries
{
    public class StudentStudyYearQueries : PaginationQuery
    {
        public int? StudyYearId { get; set; }
        public string? AcademicCode { get; set; }
        public Levels Level { get; set; }
        public int? DepartmentId { get; set; }
        public decimal? MinGPA { get; set; }
        public decimal? MaxGPA { get; set; }
        public DateTime? EnrolledFrom { get; set; }
        public DateTime? EnrolledTo { get; set; }
    }
}