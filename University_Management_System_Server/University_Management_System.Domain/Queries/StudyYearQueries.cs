using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Domain.Queries
{
    public class StudyYearQueries : PaginationQuery
    {
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
        public bool? IsCurrent { get; set; }
        public bool? HasSemesters { get; set; }
        public bool? HasRegistrations { get; set; }
        public int? MinYear { get; set; }
        public int? MaxYear { get; set; }
    }
}