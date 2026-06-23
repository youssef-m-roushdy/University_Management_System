using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Application.Dtos.StudyYearDtos
{
    public class StudyYearSemesterDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public int RegistrationsCount { get; set; }
    }
}