using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Dtos.StudentDtos
{
    public class StudentProfileDto
    {
        public string AcademicCode { get; set; } = string.Empty;
        public Levels? Level { get; set; }
        public decimal? TotalGPA { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int? SpecializationId { get; set; }
        public string? SpecializationName { get; set; }
    }
}