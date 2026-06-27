using System;
using Microsoft.AspNetCore.Http;

namespace University_Management_System.Application.Dtos.AcademicScheduleDtos
{
    public class CreateAcademicScheduleDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DepartmentId { get; set; }
        public int SemesterId { get; set; }
        public int StudyYearId { get; set; }
        public DateTime ScheduleDate { get; set; }
        public IFormFile? File { get; set; } // ✅ Use IFormFile for file upload
    }
}