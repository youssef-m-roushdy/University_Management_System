using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Domain.Queries.AssistantQueries
{
    public class AssistantDepartmentQueries : SearchablePaginationQuery
    {
        public string? Name { get; set; }
        public Gender? Gender { get; set; }
        public bool? IsActive { get; set; }
    }
}