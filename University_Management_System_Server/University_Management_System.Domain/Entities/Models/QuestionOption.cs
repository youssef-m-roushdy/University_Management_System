using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Domain.Entities.Models
{
    public class QuestionOption : BaseEntities<int>
    {
        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;

        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }
}