using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Dtos.RegistrationDtos
{
    public class UpdateRegistrationDto
    {
        public RegistrationStatus? Status { get; set; }
        public CourseProgress? Progress { get; set; }
        public string? Reason { get; set; }
        public Grades? Grade { get; set; }
    }
}