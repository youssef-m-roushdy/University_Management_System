// Student.cs — own table, one-to-one with User
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Domain.Entities.Identity
{
    public class Student : BaseEntities<string>
    {
        public User User { get; set; } = null!;

        public string AcademicCode { get; set; } = string.Empty;
        // remove all nullable becasue now student user is indepedent model except specialization because not all students have specialization
        public Levels Level { get; set; }
        public int TotalCredits { get; set; }
        public int AllowedCredits { get; set; }
        public decimal TotalGPA { get; set; } = 0.0m; // default GPA is 0.0
        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!; // means student must belong to a department, can't be null
        public int? SpecializationId { get; set; }
        public Specialization? Specialization { get; set; }
        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
        public ICollection<AcademicSchedule> AcademicSchedules { get; set; } = new List<AcademicSchedule>();
        public ICollection<CourseUpload> CourseUploads { get; set; } = new List<CourseUpload>();
        public ICollection<SemesterGPA> SemesterGPAs { get; set; } = new List<SemesterGPA>();
        public ICollection<StudentStudyYear> StudentStudyYears { get; set; } = new List<StudentStudyYear>();
    }
}