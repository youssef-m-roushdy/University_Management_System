// CourseAssistant.cs — join table (InstructorAssistant <-> Course)
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Domain.Entities.Models
{
    public class CourseAssistant : BaseEntities<int>
    {
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public int SemesterId { get; set; }
        public Semester Semester { get; set; } = null!;

        public int StudyYearId { get; set; }
        public StudyYear StudyYear { get; set; } = null!;

        public string AssistantUserId { get; set; } = string.Empty;
        public Assistant Assistant { get; set; } = null!;
    }
}