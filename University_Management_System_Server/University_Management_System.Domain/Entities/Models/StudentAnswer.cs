using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Domain.Entities.Models
{
    public class StudentAnswer : BaseEntities<int>
    {
        public int QuizAttemptId { get; set; }
        public QuizAttempt QuizAttempt { get; set; } = null!;

        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;

        // MCQ / TrueFalse
        public int? SelectedOptionId { get; set; }
        public QuestionOption? SelectedOption { get; set; }

        // ShortAnswer
        public string? AnswerText { get; set; }

        public bool? IsCorrect { get; set; }      // null until graded for ShortAnswer
        public decimal? PointsAwarded { get; set; }
    }
}