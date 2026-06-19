using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Dtos.StudentDtos
{
    public class StudentDto
    {
        // do not equal string emty becasue the values should be existed
        public string Id { get; set; }
        public string Name { get; set; }
        public string ProfilePicture { get; set; }
        public string Address { get; set; }
        public Gender Gender { get; set; }
        public string AcademicCode { get; set; }
        public Levels Level { get; set; }
        public int TotalCredits { get; set; }
        public int AllowedCredits { get; set; }
        public decimal TotalGPA { get; set; }
        public string Department { get; set; }
        public string? Specialization { get; set; }
    }
}