using System;

namespace University_Management_System.Application.Dtos.StudyYearDtos
{
    public class StudentStudyYearDto
    {
        public int Id { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string AcademicCode { get; set; } = string.Empty;
        public int Level { get; set; }
        public decimal TotalGPA { get; set; }
        public DateTime EnrolledAt { get; set; }
    }
}