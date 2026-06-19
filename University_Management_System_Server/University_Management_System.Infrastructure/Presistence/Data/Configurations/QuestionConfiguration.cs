using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Infrastructure.Presistence.Data.Configurations
{
    // QuestionConfiguration.cs
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(q => q.Id);

            builder.HasOne(q => q.Quiz)
                   .WithMany(quiz => quiz.Questions)
                   .HasForeignKey(q => q.QuizId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(q => q.Options)
                   .WithOne(o => o.Question)
                   .HasForeignKey(o => o.QuestionId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(q => q.Text)
                   .IsRequired()
                   .HasMaxLength(1000);

            builder.Property(q => q.Type)
                   .HasConversion<string>()
                   .HasMaxLength(20);

            builder.Property(q => q.Points)
                   .HasPrecision(5, 2);

            builder.Property(q => q.CorrectAnswerText)
                   .HasMaxLength(500);
        }
    }
}