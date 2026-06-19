// InstructorAssistant.cs
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Domain.Entities.Identity
{
    public class Assistant : BaseEntities<string>
    {
        public User User { get; set; } = null!;

        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!;

        // courses this assistant is assigned to
        public ICollection<CourseAssistant> CourseAssistants { get; set; } = new List<CourseAssistant>();

        // instructors this assistant is assigned to
        public ICollection<InstructorAssistant> InstructorAssistants { get; set; } = new List<InstructorAssistant>();
    }
}