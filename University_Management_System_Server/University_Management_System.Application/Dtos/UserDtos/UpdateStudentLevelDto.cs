using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Dtos.UserDtos
{
    public class UpdateStudentLevelDto
    {
        public string? AcademicCode { get; set; }
        public Levels Level { get; set; }
    }
}