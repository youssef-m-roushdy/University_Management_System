using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Entities.Identity;

namespace University_Management_System.Domain.Entities.Models
{
    public class QuizAttempt : BaseEntities<int>
    {
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; } = null!;

        public string StudentId { get; set; } = string.Empty;
        public Student Student { get; set; } = null!;

        public int AttemptNumber { get; set; } = 1; // supports MaxAttempts > 1
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public DateTime? SubmittedAt { get; set; }
        public decimal? Score { get; set; }
        public bool IsGraded { get; set; } = false;

        public ICollection<StudentAnswer> Answers { get; set; } = new List<StudentAnswer>();
    }
}