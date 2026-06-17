using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Application.Dtos.UserDtos
{
    public class UpdateStudentAllowedCreditsDto
    {
        public string? AcademicCode { get; set; }
        public int AllowedCredits { get; set; }
    }
}