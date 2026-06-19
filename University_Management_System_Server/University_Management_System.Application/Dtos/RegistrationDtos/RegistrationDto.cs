using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.CourseDtos;
using University_Management_System.Application.Dtos.StudentDtos;
using University_Management_System.Application.Dtos.UserDtos;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Dtos.RegistrationDtos
{
    public class RegistrationDto
    {
        public int Id { get; set; }
        public RegistrationStatus Status { get; set; }
        public CourseProgress Progress { get; set; }
        public string? Reason { get; set; } // the reason for pending or canceling the registration
        public Grades? Grade { get; set; } // null if the course is not yet completed, otherwise it holds the grade received
        public bool IsPassed { get; set; } // This property indicates whether the course has been passed by the student or not. It can be used to determine if the student has met the prerequisites for other courses. 
        public StudentDto Student { get; set; } = null!;
        public CourseDto Course { get; set; } = null!;
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    }
}