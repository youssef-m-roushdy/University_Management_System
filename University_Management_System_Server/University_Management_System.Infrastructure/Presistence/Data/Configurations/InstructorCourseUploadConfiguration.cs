using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Infrastructure.Presistence.Data.Configurations
{
    public class InstructorCourseUploadConfiguration : IEntityTypeConfiguration<InstructorCourseUpload>
    {
        public void Configure(EntityTypeBuilder<InstructorCourseUpload> builder)
        {
            builder.HasKey(i => i.Id);

            builder.HasOne(i => i.Instructor)
                .WithMany()
                .HasForeignKey(i => i.InstructorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(i => i.Semester)
                .WithMany()
                .HasForeignKey(i => i.SemesterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.StudyYear)
                .WithMany()
                .HasForeignKey(i => i.StudyYearId)
                .OnDelete(DeleteBehavior.Restrict);

            // M:M → EF Core creates a join table "InstructorCourseUploadItems"
            builder.HasMany(i => i.CourseUploads)
                .WithMany(cu => cu.InstructorCourseUploads)
                .UsingEntity<Dictionary<string, object>>(
                    "InstructorCourseUploadItems",
                    r => r.HasOne<CourseUpload>()
                            .WithMany()
                            .HasForeignKey("CourseUploadId")
                            .OnDelete(DeleteBehavior.Cascade),
                    l => l.HasOne<InstructorCourseUpload>()
                            .WithMany()
                            .HasForeignKey("InstructorCourseUploadId")
                            .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasKey("InstructorCourseUploadId", "CourseUploadId")
                );

            // prevent duplicate group per instructor per semester per year
            builder.HasIndex(i => new { i.InstructorId, i.SemesterId, i.StudyYearId })
                .IsUnique();
        }
    }
}