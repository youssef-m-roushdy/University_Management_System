using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using University_Management_System.Domain.Entities.Models;
namespace University_Management_System.Infrastructure.Presistence.Data.Configurations
{
    public class AssistantCourseUploadConfiguration : IEntityTypeConfiguration<AssistantCourseUpload>
    {
        public void Configure(EntityTypeBuilder<AssistantCourseUpload> builder)
        {
            builder.HasKey(a => a.Id);

            builder.HasOne(a => a.Assistant)
                .WithMany()
                .HasForeignKey(a => a.AssistantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Semester)
                .WithMany()
                .HasForeignKey(a => a.SemesterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.StudyYear)
                .WithMany()
                .HasForeignKey(a => a.StudyYearId)
                .OnDelete(DeleteBehavior.Restrict);

            // M:M → EF Core creates a join table "AssistantCourseUploadItems"
            builder.HasMany(a => a.CourseUploads)
                .WithMany(cu => cu.AssistantCourseUploads)
                .UsingEntity<Dictionary<string, object>>(
                    "AssistantCourseUploadItems",
                    r => r.HasOne<CourseUpload>()
                            .WithMany()
                            .HasForeignKey("CourseUploadId")
                            .OnDelete(DeleteBehavior.Cascade),
                    l => l.HasOne<AssistantCourseUpload>()
                            .WithMany()
                            .HasForeignKey("AssistantCourseUploadId")
                            .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasKey("AssistantCourseUploadId", "CourseUploadId")
                );

            builder.HasIndex(a => new { a.AssistantId, a.SemesterId, a.StudyYearId })
                .IsUnique();
        }
    }
}