using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Dtos.StudentDtos
{
    public class UpdateStudentDto
    {
        public string AcademicCode { get; set; } = string.Empty;
        public Levels Level { get; set; }
        public int TotalCredits { get; set; }
        public int AllowedCredits { get; set; }
        public decimal TotalGPA { get; set; } = 0.0m; // default GPA is 0.0
        public int DepartmentId { get; set; }
        public int? SpecializationId { get; set; }
    }
}