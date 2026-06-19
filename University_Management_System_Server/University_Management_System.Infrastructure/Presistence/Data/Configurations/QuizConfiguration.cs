using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Infrastructure.Presistence.Data.Configurations
{
    // QuizConfiguration.cs
    public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            builder.HasKey(q => q.Id);

            builder.HasOne(q => q.Course)
                   .WithMany()
                   .HasForeignKey(q => q.CourseId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(q => q.Semester)
                   .WithMany()
                   .HasForeignKey(q => q.SemesterId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(q => q.StudyYear)
                   .WithMany()
                   .HasForeignKey(q => q.StudyYearId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(q => q.Instructor)
                   .WithMany()
                   .HasForeignKey(q => q.InstructorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(q => q.InstructorAssistant)
                   .WithMany()
                   .HasForeignKey(q => q.AssistantId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(q => q.Questions)
                   .WithOne(qs => qs.Quiz)
                   .HasForeignKey(qs => qs.QuizId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(q => q.Attempts)
                   .WithOne(a => a.Quiz)
                   .HasForeignKey(a => a.QuizId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(q => q.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(q => q.Description)
                   .HasMaxLength(1000);

            builder.Property(q => q.TotalScore)
                   .HasPrecision(6, 2);

            builder.ToTable(t => t.HasCheckConstraint(
                "CK_Quiz_CreatorExclusive",
                "(\"InstructorId\" IS NOT NULL AND \"AssistantId\" IS NULL) OR (\"InstructorId\" IS NULL AND \"AssistantId\" IS NOT NULL)"
            ));

            builder.ToTable(t => t.HasCheckConstraint(
                "CK_Quiz_EndAfterStart",
                "\"EndTime\" > \"StartTime\""
            ));
        }
    }
}