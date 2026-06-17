
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Application.Dtos.RegistrationDtos
{
    public class CreateRegistrationDto
    {
        public int CourseId { get; set; }
        public int StudyYearId { get; set; }
        public int SemesterId { get; set; }
    }
}