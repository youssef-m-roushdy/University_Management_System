// Instructor.cs
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Domain.Entities.Identity
{
    public class Instructor : BaseEntities<string>
    {
        public User User { get; set; } = null!;

        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!;
        // courses this instructor teaches
        public ICollection<CourseInstructor> CourseInstructors { get; set; } = new List<CourseInstructor>();

        // assistants under this instructor
        public ICollection<InstructorAssistant> InstructorAssistants { get; set; } = new List<InstructorAssistant>();
    }
}