using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Infrastructure.Presistence.Data.Configurations
{
    // QuizAttemptConfiguration.cs
    public class QuizAttemptConfiguration : IEntityTypeConfiguration<QuizAttempt>
    {
        public void Configure(EntityTypeBuilder<QuizAttempt> builder)
        {
            builder.HasKey(a => a.Id);

            builder.HasOne(a => a.Quiz)
                   .WithMany(q => q.Attempts)
                   .HasForeignKey(a => a.QuizId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Student)
                   .WithMany()
                   .HasForeignKey(a => a.StudentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.Answers)
                   .WithOne(ans => ans.QuizAttempt)
                   .HasForeignKey(ans => ans.QuizAttemptId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(a => a.Score)
                   .HasPrecision(6, 2);

            // prevents duplicate attempt numbers per student per quiz
            builder.HasIndex(a => new { a.QuizId, a.StudentId, a.AttemptNumber })
                   .IsUnique();
        }
    }
}