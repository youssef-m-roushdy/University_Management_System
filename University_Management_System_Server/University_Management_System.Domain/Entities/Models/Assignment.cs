// Assignment.cs
using University_Management_System.Domain.Entities;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Domain.Entities.Models
{
    public class Assignment : BaseEntities<int>
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Url { get; set; } = string.Empty; // assignment must be not null, it can be a file or a link to the assignment

        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public int SemesterId { get; set; }
        public Semester Semester { get; set; } = null!;

        public int StudyYearId { get; set; }
        public StudyYear StudyYear { get; set; } = null!;

        // creator — either Instructor or Assistant, never both (enforced at config/service level)
        public string? InstructorId { get; set; }
        public Instructor? Instructor { get; set; }

        public string? AssistantId { get; set; }
        public Assistant? Assistant { get; set; }

        public DateTime DueDate { get; set; }
        public decimal MaxScore { get; set; }
        public bool AllowLateSubmission { get; set; } = false;
        public ICollection<AssignmentSubmission> Submissions { get; set; } = new List<AssignmentSubmission>();
    }
}