using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Entities.Identity;

namespace University_Management_System.Domain.Entities.Models
{
    public class Quiz : BaseEntities<int>
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public int SemesterId { get; set; }
        public Semester Semester { get; set; } = null!;

        public int StudyYearId { get; set; }
        public StudyYear StudyYear { get; set; } = null!;

        public string? InstructorId { get; set; }
        public Instructor? Instructor { get; set; }

        public string? AssistantId { get; set; }
        public Assistant? InstructorAssistant { get; set; }

        public DateTime StartTime { get; set; }   // quiz window opens
        public DateTime EndTime { get; set; }     // quiz window closes
        public int DurationMinutes { get; set; }  // time limit once a student starts
        public decimal TotalScore { get; set; }
        public int MaxAttempts { get; set; } = 1;
        public bool IsPublished { get; set; } = false;
        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<QuizAttempt> Attempts { get; set; } = new List<QuizAttempt>();
    }
}