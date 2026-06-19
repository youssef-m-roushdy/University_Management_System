using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Infrastructure.Presistence.Data.Configurations
{
    // AssignmentConfiguration.cs
    public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> builder)
        {
            builder.HasKey(a => a.Id);

            builder.HasOne(a => a.Course)
                   .WithMany()
                   .HasForeignKey(a => a.CourseId)
                   .OnDelete(DeleteBehavior.Cascade); // assignment dies with course

            builder.HasOne(a => a.Semester)
                   .WithMany()
                   .HasForeignKey(a => a.SemesterId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.StudyYear)
                   .WithMany()
                   .HasForeignKey(a => a.StudyYearId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Instructor)
                   .WithMany()
                   .HasForeignKey(a => a.InstructorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Assistant)
                   .WithMany()
                   .HasForeignKey(a => a.AssistantId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.Submissions)
                   .WithOne(s => s.Assignment)
                   .HasForeignKey(s => s.AssignmentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(a => a.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(a => a.Description)
                   .HasMaxLength(2000);


            builder.Property(a => a.Url)
                   .HasMaxLength(500);

            builder.Property(a => a.MaxScore)
                   .HasPrecision(6, 2);

            // either Instructor or Assistant must be set, never both
            builder.ToTable(t => t.HasCheckConstraint(
                "CK_Assignment_CreatorExclusive",
                "(\"InstructorId\" IS NOT NULL AND \"AssistantId\" IS NULL) OR (\"InstructorId\" IS NULL AND \"AssistantId\" IS NOT NULL)"
            ));
        }
    }
}