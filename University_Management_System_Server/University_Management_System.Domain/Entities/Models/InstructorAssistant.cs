using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Entities.Identity;

namespace University_Management_System.Domain.Entities.Models
{
    public class InstructorAssistant : BaseEntities<int>
    {
        // here the relation where m to m relation between instructor and assistant, because an instructor can have many assistants and an assistant can be supervised by many instructors
        // to specicfic year and semester
        public string InstructorId { get; set; } = string.Empty;
        public Instructor Instructor { get; set; } = null!;
        public string AssistantId { get; set; } = string.Empty;
        public Assistant Assistant { get; set; } = null!;
        public int SemesterId { get; set; }
        public Semester Semester { get; set; } = null!;
        public int StudyYearId { get; set; }
        public StudyYear StudyYear { get; set; } = null!;
    }
}