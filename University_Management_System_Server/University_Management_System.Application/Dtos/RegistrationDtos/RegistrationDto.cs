using System;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Dtos.RegistrationDtos
{
    public class RegistrationDto
    {
        public int Id { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string AcademicCode { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public int Credits { get; set; }
        public int SemesterId { get; set; }
        public string SemesterTitle { get; set; } = string.Empty;
        public int StudyYearId { get; set; }
        public string StudyYearRange { get; set; } = string.Empty;
        public RegistrationStatus Status { get; set; }
        public CourseProgress Progress { get; set; }
        public Grades? Grade { get; set; }
        public bool IsPassed { get; set; }
        public string? Reason { get; set; }
        public DateTime RegisteredAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}