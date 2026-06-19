using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Dtos.StudentStudyYearDtos
{
    /// <summary>
    /// Full timeline of a student's study years from enrollment to graduation.
    /// </summary>
    public class StudentStudyYearTimelineDto
    {
        public string StudentId { get; set; } = string.Empty;
        public Levels CurrentLevel { get; set; }
        public string Department { get; set; } = string.Empty;
        public int TotalYearsCompleted { get; set; }
        public bool IsGraduated { get; set; }
        public List<StudentStudyYearDetailsDto> StudyYears { get; set; } = new();
    }
}
