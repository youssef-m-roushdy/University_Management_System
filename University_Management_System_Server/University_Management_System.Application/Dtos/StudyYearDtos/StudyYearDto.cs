using System;
using System.Collections.Generic;

namespace University_Management_System.Application.Dtos.StudyYearDtos
{
    public class StudyYearDto
    {
        public int Id { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public bool IsCurrent { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? SemesterCount { get; set; }
        public int? StudentCount { get; set; }
        public int? RegistrationCount { get; set; }
    }
}