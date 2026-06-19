using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Domain.Entities.Models
{
    public class Question : BaseEntities<int>
    {
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; } = null!;

        public string Text { get; set; } = string.Empty;
        public QuestionType Type { get; set; }
        public decimal Points { get; set; }

        // manually graded by instructor/assistant if Type == ShortAnswer
        public string? CorrectAnswerText { get; set; }
        public ICollection<QuestionOption> Options { get; set; } = new List<QuestionOption>();
    }
}