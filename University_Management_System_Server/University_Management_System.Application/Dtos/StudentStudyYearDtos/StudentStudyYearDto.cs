using System;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Dtos.StudentStudyYearDtos
{
    public class StudentStudyYearDto
    {
        // ─── IDs (For Navigation & API Calls) ──────────────────────────────────
        public int Id { get; set; }                    // Enrollment ID
        public string StudentId { get; set; } = string.Empty;      // For navigation to student
        public int StudyYearId { get; set; }           // For navigation to study year
        public string? SemesterId { get; set; }        // For navigation to semester (optional)
        
        // ─── Student Info (Display) ─────────────────────────────────────────────
        public string AcademicCode { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string? Email { get; set; }
        
        // ─── Department Info (Display) ──────────────────────────────────────────
        public string? DepartmentName { get; set; }
        public string? DepartmentCode { get; set; }
        public int? DepartmentId { get; set; }  // For navigation to department
        
        // ─── Specialization Info (Display) ──────────────────────────────────────
        public string? SpecializationName { get; set; }
        public int? SpecializationId { get; set; }  // For navigation to specialization
        
        // ─── Study Year Info (Display) ──────────────────────────────────────────
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public string YearRange => $"{StartYear}-{EndYear}";
        public bool IsCurrentStudyYear { get; set; }
        
        // ─── Academic Info (Display) ────────────────────────────────────────────
        public Levels Level { get; set; }
        public decimal? TotalGPA { get; set; }
        public int? TotalCredits { get; set; }
        public int? AllowedCredits { get; set; }
        
        // ─── Status Info (Display) ──────────────────────────────────────────────
        public bool IsActive { get; set; }
        public string Status => IsActive ? "Active" : "Inactive";
        
        // ─── Dates (Display) ────────────────────────────────────────────────────
        public DateTime EnrolledAt { get; set; }
        public DateTime? GraduatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}