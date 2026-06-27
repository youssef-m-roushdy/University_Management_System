using System;

namespace University_Management_System.Application.Dtos.AcademicScheduleDtos
{
    public class AcademicScheduleDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string DepartmentCode { get; set; } = string.Empty;
        public int SemesterId { get; set; }
        public string SemesterTitle { get; set; } = string.Empty;
        public int StudyYearId { get; set; }
        public string StudyYearRange { get; set; } = string.Empty;
        public string? AdminName { get; set; }
        public DateTime ScheduleDate { get; set; }
        public string ScheduleDateDisplay { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}