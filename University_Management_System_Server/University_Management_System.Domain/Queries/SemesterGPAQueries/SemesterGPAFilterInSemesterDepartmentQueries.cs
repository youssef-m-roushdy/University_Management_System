using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Domain.Queries.SemesterGPAQueries
{
    public class SemesterGPAFilterInSemesterDepartmentQueries
    {
        public int? StudyYearId { get; set; }
        public decimal? MinGPA { get; set; }
        public decimal? MaxGPA { get; set; }
        public int? MinCreditHours { get; set; }
        public int? MaxCreditHours { get; set; }
        public DateTime? CalculatedFrom { get; set; }
        public DateTime? CalculatedTo { get; set; }
    }
}