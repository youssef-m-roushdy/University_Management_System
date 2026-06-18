// Instructor.cs
using University_Management_System.Domain.Entities.Models;

public class Instructor
{
    public string UserId { get; set; } = string.Empty; // FK + PK
    public User User { get; set; } = null!;

    public int DepartmentId { get; set; }
    public Department Department { get; set; } = null!;

    // courses this instructor teaches
    public ICollection<CourseInstructor> CourseInstructors { get; set; } = new List<CourseInstructor>();

    // assistants under this instructor
    public ICollection<Assistant> InstructorAssistants { get; set; } = new List<Assistant>();
}