using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Infrastructure.Presistence.Data.Configurations
{
    // AssignmentSubmissionConfiguration.cs
    public class AssignmentSubmissionConfiguration : IEntityTypeConfiguration<AssignmentSubmission>
    {
        public void Configure(EntityTypeBuilder<AssignmentSubmission> builder)
        {
            builder.HasKey(s => s.Id);

            builder.HasOne(s => s.Assignment)
                   .WithMany(a => a.Submissions)
                   .HasForeignKey(s => s.AssignmentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.Student)
                   .WithMany()
                   .HasForeignKey(s => s.StudentId)
                   .OnDelete(DeleteBehavior.Cascade); // submission dies with student

            builder.HasOne(s => s.GradedByInstructor)
                   .WithMany()
                   .HasForeignKey(s => s.GradedByInstructorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.GradedByAssistant)
                   .WithMany()
                   .HasForeignKey(s => s.GradedByAssistantId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(s => s.Url)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(s => s.Score)
                   .HasPrecision(6, 2);

            builder.Property(s => s.Feedback)
                   .HasMaxLength(1000);

            builder.Property(s => s.Status)
                   .HasConversion<string>()
                   .HasMaxLength(20);

            // one submission per student per assignment
            builder.HasIndex(s => new { s.AssignmentId, s.StudentId })
                   .IsUnique();
        }
    }
}