// InstructorAssistant.cs
using University_Management_System.Domain.Entities.Models;

public class Assistant
{
    public string UserId { get; set; } = string.Empty; // FK + PK
    public User User { get; set; } = null!;

    public int DepartmentId { get; set; }
    public Department Department { get; set; } = null!;

    // supervised by a specific instructor
    public string InstructorId { get; set; } = string.Empty;
    public Instructor Instructor { get; set; } = null!;

    // courses this assistant is assigned to
    public ICollection<CourseAssistant> CourseAssistants { get; set; } = new List<CourseAssistant>();
}