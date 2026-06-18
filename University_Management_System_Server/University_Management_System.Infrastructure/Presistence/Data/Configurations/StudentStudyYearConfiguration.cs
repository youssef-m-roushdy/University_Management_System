using University_Management_System.Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace University_Management_System.Infrastructure.Presistence.Data.Configurations
{
    public class StudentStudyYearConfiguration : IEntityTypeConfiguration<StudentStudyYear>
    {
        public void Configure(EntityTypeBuilder<StudentStudyYear> builder)
        {
            builder.HasKey(usy => usy.Id);

            // Unique constraint: one record per user per study year
            builder.HasIndex(usy => new { usy.StudentId, usy.StudyYearId })
                   .IsUnique();

            builder.Property(usy => usy.StudentId)
                   .IsRequired()
                   .HasMaxLength(450);

            builder.Property(usy => usy.Level)
                   .IsRequired();

            builder.HasOne(usy => usy.Student)
                   .WithMany(s => s.StudentStudyYears)
                   .HasForeignKey(usy => usy.StudentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(usy => usy.StudyYear)
                   .WithMany(sy => sy.StudentStudyYears)
                   .HasForeignKey(usy => usy.StudyYearId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
