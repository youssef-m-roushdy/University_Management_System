using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Domain.Queries.SpecializationQueries
{
    public class SpecializationFilterQueries : SearchablePaginationQuery
    {
        public int? DepartmentId { get; set; }
        public string? Name { get; set; }
        public bool? HasStudents { get; set; }
        public bool? HasCourses { get; set; }
    }
}