using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Domain.Queries.InstructorQueries
{
    public class InstructorFilterQueries : SearchablePaginationQuery
    {
        public string? Name { get; set; }
        public Gender? Gender { get; set; }
        public string? DepartmentSearch { get; set; }
        public bool? IsActive { get; set; }
    }
}