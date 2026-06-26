using System.Collections.Generic;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Dtos.DepartmentCourseDtos
{
    public class CreateDepartmentCourseBulkDto
    {
        public int DepartmentId { get; set; }
        public List<int> CourseIds { get; set; } = new List<int>();
        public CourseRole Role { get; set; }
    }
}