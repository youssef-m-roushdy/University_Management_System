using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Infrastructure.Presistence.Data.Configurations
{
    // StudentAnswerConfiguration.cs
    public class StudentAnswerConfiguration : IEntityTypeConfiguration<StudentAnswer>
    {
        public void Configure(EntityTypeBuilder<StudentAnswer> builder)
        {
            builder.HasKey(a => a.Id);

            builder.HasOne(a => a.QuizAttempt)
                   .WithMany(qa => qa.Answers)
                   .HasForeignKey(a => a.QuizAttemptId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Question)
                   .WithMany()
                   .HasForeignKey(a => a.QuestionId)
                   .OnDelete(DeleteBehavior.Restrict); // don't cascade-delete answers if question edited/removed mid-attempt

            builder.HasOne(a => a.SelectedOption)
                   .WithMany()
                   .HasForeignKey(a => a.SelectedOptionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(a => a.AnswerText)
                   .HasMaxLength(1000);

            builder.Property(a => a.PointsAwarded)
                   .HasPrecision(5, 2);

            // one answer per question per attempt
            builder.HasIndex(a => new { a.QuizAttemptId, a.QuestionId })
                   .IsUnique();
        }
    }
}