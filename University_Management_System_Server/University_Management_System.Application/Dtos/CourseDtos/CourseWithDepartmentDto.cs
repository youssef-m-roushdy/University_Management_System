using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Dtos.CourseDtos
{
    public class CourseWithDepartmentDto
    {
        public int Id {get; set;}
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public CourseStatus Status { get; set; }
        public int Credits { get; set; }
        public string Department { get; set; } = string.Empty;
    }
}