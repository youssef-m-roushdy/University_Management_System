using System.ComponentModel.DataAnnotations;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Dtos.StudentStudyYearDtos
{
    public class CreateStudentStudyYearDto
    {
        public string StudentId { get; set; } = string.Empty;
        public int StudyYearId { get; set; }
        public Levels Level { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
