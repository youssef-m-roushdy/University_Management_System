using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Application.Dtos.UserDtos
{
    public class UpdateStudentTotalCreditsDto
    {
        public string? AcademicCode { get; set; } // null if 
        public int TotalCredits { get; set; }
    }
}