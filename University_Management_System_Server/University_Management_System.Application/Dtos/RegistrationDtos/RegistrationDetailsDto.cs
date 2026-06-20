using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.CourseDtos;
using University_Management_System.Application.Dtos.SemesterDtos;
using University_Management_System.Application.Dtos.StudyYearDtos;
using University_Management_System.Application.Dtos.UserDtos;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Dtos.RegistrationDtos
{
    public class RegistrationDetailsDto
    {
        public int Id { get; set; }
        public RegistrationStatus Status { get; set; }
        public string? Reason { get; set; }
        public Grades? Grade { get; set; }
        public CourseDto Course { get; set; } = null!;
        public StudyYearDto StudyYearDto { get; set; } = null!;
        public SemesterDto Semester { get; set; } = null!;
        public UserBasicDto User { get; set; } = null!;
        public CourseDto CourseDto { get; set; } = null!;
        public DateTime RegisteredAt { get; set; }
    }
}