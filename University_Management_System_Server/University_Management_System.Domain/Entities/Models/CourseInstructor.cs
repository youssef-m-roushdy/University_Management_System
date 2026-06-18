// CourseInstructor.cs — join table (Instructor <-> Course)
using University_Management_System.Domain.Entities.Models;

public class CourseInstructor
{
    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;

    public string InstructorUserId { get; set; } = string.Empty;
    public Instructor Instructor { get; set; } = null!;

    public int SemesterId { get; set; }
    public Semester Semester { get; set; } = null!;

    public int StudyYearId { get; set; }
    public StudyYear StudyYear { get; set; } = null!;

    public bool IsPrimary { get; set; } = true; // a course might have a primary + co-instructor

    public ICollection<InstructorCourseUpload> InstructorCourseUploads { get; set; } = new List<InstructorCourseUpload>();
}