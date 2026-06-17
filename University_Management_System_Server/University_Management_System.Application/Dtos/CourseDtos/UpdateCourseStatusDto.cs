using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Dtos.CourseDtos
{
    public class UpdateCourseStatusDto
    {
        public int CourseId { get; set; }
        public CourseStatus Status { get; set; }
    }
}