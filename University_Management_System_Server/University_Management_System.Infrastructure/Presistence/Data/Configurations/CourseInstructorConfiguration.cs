using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Infrastructure.Presistence.Data.Configurations
{
    public class CourseInstructorConfiguration : IEntityTypeConfiguration<CourseInstructor>
    {
        public void Configure(EntityTypeBuilder<CourseInstructor> builder)
        {
            builder.HasKey(ci => new { ci.CourseId, ci.InstructorUserId, ci.SemesterId, ci.StudyYearId });

            builder.HasOne(ci => ci.Course)
                .WithMany()
                .HasForeignKey(ci => ci.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ci => ci.Instructor)
                .WithMany(i => i.CourseInstructors)
                .HasForeignKey(ci => ci.InstructorUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ci => ci.Semester)
                .WithMany()
                .HasForeignKey(ci => ci.SemesterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ci => ci.StudyYear)
                .WithMany(sy => sy.CourseInstructors)
                .HasForeignKey(ci => ci.StudyYearId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
