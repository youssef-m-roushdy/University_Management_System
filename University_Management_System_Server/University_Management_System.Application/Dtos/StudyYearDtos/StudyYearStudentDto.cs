using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Application.Dtos.StudyYearDtos
{
    public class StudyYearStudentDto
    {
        public string StudentId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string AcademicCode { get; set; } = string.Empty;
        public int Level { get; set; }
        public decimal TotalGPA { get; set; }
        public DateTime EnrolledAt { get; set; }
    }
}