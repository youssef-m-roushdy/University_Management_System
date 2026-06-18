using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Domain.Entities.Models
{
    public class StudyYear : BaseEntities<int>
    {
        public int StartYear { get; set; } // e.g., 2024, 2025, etc.
        public int EndYear { get; set; } // e.g., 2025, 2026, etc. means if the term starts in 2024 and ends in 2025, then StartYear = 2024 and EndYear = 2025
        public bool IsCurrent { get; set; } // Indicates if this is the current study year
        public ICollection<Fee> Fees { get; set; } = new List<Fee>(); // means the study year has multiple fees for different levels and departments
        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
        public ICollection<Semester> Semesters { get; set; } = new List<Semester>();
        public ICollection<SemesterGPA> SemesterGPAs { get; set; } = new List<SemesterGPA>();
        public ICollection<AcademicSchedule> AcademicSchedules { get; set; } = new List<AcademicSchedule>();
        public ICollection<StudentStudyYear> StudentStudyYears { get; set; } = new List<StudentStudyYear>();
        public ICollection<CourseAssistant> CourseAssistants { get; set; } = new List<CourseAssistant>();
        public ICollection<CourseInstructor> CourseInstructors { get; set; } = new List<CourseInstructor>();
        public ICollection<InstructorCourseUpload> InstructorCourseUploads { get; set; } = new List<InstructorCourseUpload>();
        public ICollection<AssistantCourseUpload> AssistantCourseUploads { get; set; } = new List<AssistantCourseUpload>();
    }
}