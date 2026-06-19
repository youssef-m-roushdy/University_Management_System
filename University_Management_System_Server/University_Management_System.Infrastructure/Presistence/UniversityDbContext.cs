using System.Reflection;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Domain.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace University_Management_System.Infrastructure.Presistence
{
    public class UniversityDbContext : IdentityDbContext<User, Role, string,
    IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>,
    IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public UniversityDbContext(DbContextOptions<UniversityDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId);

                userRole.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId);
            });

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            builder.Entity<Student>()
                   .HasIndex(u => u.AcademicCode)
                   .IsUnique();
        }

        // Identity
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        // Domain tables
        public DbSet<Department> Departments { get; set; }
        public DbSet<StudyYear> StudyYears { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<AcademicSchedule> AcademicSchedules { get; set; }
        public DbSet<Fee> Fees { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CoursePrerequisite> CoursePrerequisites { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<CourseUpload> CourseUploads { get; set; }
        public DbSet<SemesterGPA> SemesterGPAs { get; set; }
        public DbSet<StudentStudyYear> StudentStudyYears { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<SpecializationCourse> SpecializationCourses { get; set; }
        public DbSet<DepartmentCourse> DepartmentCourses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Assistant> Assistants { get; set; }
        public DbSet<InstructorCourseUpload> InstructorCourseUploads { get; set; }
        public DbSet<AssistantCourseUpload> AssistantCourseUploads { get; set; }
        public DbSet<CourseInstructor> CourseInstructors { get; set; }
        public DbSet<CourseAssistant> CourseAssistants { get; set; }

        // ===== Instructor ↔ Assistant Supervision (M:M, time-scoped) =====
        public DbSet<InstructorAssistant> InstructorAssistant { get; set; }

        // ===== Assignments =====
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<AssignmentSubmission> AssignmentSubmissions { get; set; }

        // ===== Quizzes =====
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }
        public DbSet<QuizAttempt> QuizAttempts { get; set; }
        public DbSet<StudentAnswer> StudentAnswers { get; set; }
    }

}
