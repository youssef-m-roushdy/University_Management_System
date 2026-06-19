using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Domain.Queries
{
    public class UserQueries : PaginationQuery
    {
        public string? AcademicCode { get; set; }
        public string? Name { get; set; }
        public Gender? Gender { get; set; }
        public Levels? Level { get; set; } 
        public int? DepartmentId { get; set; }
        public string? Role { get; set; }
    }
}