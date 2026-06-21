using System;

namespace University_Management_System.Application.Dtos.StudyYearDtos
{
    public class UpdateStudyYearDto
    {
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public bool IsCurrent { get; set; }
    }
}