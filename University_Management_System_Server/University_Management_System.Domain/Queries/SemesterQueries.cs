using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Domain.Queries
{
    public class SemesterQueries : PaginationQuery // no searchable term for semester, so we don't need to inherit from SearchablePaginationQuery
    {
        public int? StudyYearId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? StartDateFrom { get; set; }
        public DateTime? StartDateTo { get; set; }
        public DateTime? EndDateFrom { get; set; }
        public DateTime? EndDateTo { get; set; }
    }
}