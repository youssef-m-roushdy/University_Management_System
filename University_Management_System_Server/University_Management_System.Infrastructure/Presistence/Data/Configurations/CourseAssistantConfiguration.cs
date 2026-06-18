using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Infrastructure.Presistence.Data.Configurations
{
    public class CourseAssistantConfiguration : IEntityTypeConfiguration<CourseAssistant>
    {
        public void Configure(EntityTypeBuilder<CourseAssistant> builder)
        {
            builder.HasKey(ca => new { ca.CourseId, ca.AssistantUserId, ca.SemesterId, ca.StudyYearId });

            builder.HasOne(ca => ca.Course)
                .WithMany()
                .HasForeignKey(ca => ca.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ca => ca.Assistant)
                .WithMany(a => a.CourseAssistants)
                .HasForeignKey(ca => ca.AssistantUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ca => ca.Semester)
                .WithMany()
                .HasForeignKey(ca => ca.SemesterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ca => ca.StudyYear)
                .WithMany(sy => sy.CourseAssistants)
                .HasForeignKey(ca => ca.StudyYearId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
