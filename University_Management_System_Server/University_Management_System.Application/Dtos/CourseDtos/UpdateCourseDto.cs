using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Dtos.CourseDtos
{
    public class UpdateCourseDto
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Credits { get; set; }
        public CourseStatus? Status { get; set; }
        public int? DepartmentId { get; set; }
    }
}