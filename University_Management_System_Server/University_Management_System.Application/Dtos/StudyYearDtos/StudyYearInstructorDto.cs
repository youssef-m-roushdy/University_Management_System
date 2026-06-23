using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Application.Dtos.StudyYearDtos
{
    public class StudyYearInstructorDto
    {
        public string InstructorId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public int CoursesCount { get; set; }
    }
}