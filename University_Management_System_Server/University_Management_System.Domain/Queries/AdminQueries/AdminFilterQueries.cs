using System;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Domain.Queries.AdminQueries
{
    public class AdminFilterQueries : SearchablePaginationQuery
    {
        public string? Name { get; set; }
        public Gender? Gender { get; set; }
        public bool? IsActive { get; set; }
    }
}