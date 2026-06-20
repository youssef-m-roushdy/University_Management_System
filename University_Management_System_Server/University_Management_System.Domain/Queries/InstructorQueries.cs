using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Domain.Queries
{
    public class InstructorQueries : PaginationQuery
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public Gender? Gender { get; set; }
        public int? DepartmentId { get; set; }
        public string? Title { get; set; }
        public bool? IsActive { get; set; }
    }
}