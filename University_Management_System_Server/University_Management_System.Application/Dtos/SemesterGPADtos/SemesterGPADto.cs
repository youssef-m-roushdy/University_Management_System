using System;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Dtos.SemesterGPADtos
{
    public class SemesterGPADto
    {
        public int Id { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string AcademicCode { get; set; } = string.Empty;
        public int SemesterId { get; set; }
        public SemesterEnum SemesterTitle { get; set; }
        public int StudyYearId { get; set; }
        public string StudyYearRange { get; set; } = string.Empty;
        public decimal GPA { get; set; }
        public int TotalCreditHours { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string DepartmentCode { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}