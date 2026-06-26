using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Domain.Queries.SpecializationCourseQueries
{
    public class CourseFilterInSpecailizationQueries : SearchablePaginationQuery
    {
        public int? SpecializationId { get; set; }
        public int? CourseId { get; set; }
        public string? CourseName { get; set; }
        public string? CourseCode { get; set; }
        public int? MinCredits { get; set; }
        public int? MaxCredits { get; set; }
        public bool? HasPrerequisites { get; set; }
    }
}