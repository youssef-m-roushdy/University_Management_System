// Student.cs — own table, one-to-one with User
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Enums;

public class Student
{
    public string UserId { get; set; } = string.Empty; // FK + PK
    public User User { get; set; } = null!;

    public string AcademicCode { get; set; } = string.Empty;
    public Levels? Level { get; set; }
    public int? TotalCredits { get; set; }
    public int? AllowedCredits { get; set; }
    public decimal? TotalGPA { get; set; }

    public int? DepartmentId { get; set; }
    public Department? Department { get; set; }

    public int? SpecializationId { get; set; }
    public Specialization? Specialization { get; set; }

    public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    public ICollection<AcademicSchedule> AcademicSchedules { get; set; } = new List<AcademicSchedule>();
    public ICollection<CourseUpload> CourseUploads { get; set; } = new List<CourseUpload>();
    public ICollection<SemesterGPA> SemesterGPAs { get; set; } = new List<SemesterGPA>();
    public ICollection<StudentStudyYear> StudentStudyYears { get; set; } = new List<StudentStudyYear>();
}