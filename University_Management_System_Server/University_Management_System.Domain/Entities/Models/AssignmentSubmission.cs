// AssignmentSubmission.cs
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Domain.Entities.Models
{
    public class AssignmentSubmission : BaseEntities<int>
    {
        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; } = null!;

        public string StudentId { get; set; } = string.Empty;
        public Student Student { get; set; } = null!;
        public string Url { get; set; } = string.Empty; // submission file URL or link to the submission no need to fileId already fileId exist in url link

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
        public SubmissionStatus Status { get; set; } = SubmissionStatus.Submitted;

        public decimal? Score { get; set; }
        public string? Feedback { get; set; }
        public DateTime? GradedAt { get; set; }

        // grader — Instructor or Assistant
        public string? GradedByInstructorId { get; set; }
        public Instructor? GradedByInstructor { get; set; }

        public string? GradedByAssistantId { get; set; }
        public Assistant? GradedByAssistant { get; set; }
    }
}