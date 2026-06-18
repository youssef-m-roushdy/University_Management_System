using University_Management_System.Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace University_Management_System.Infrastructure.Presistence.Data.Configurations
{
    public class RegistrationConfiguration : IEntityTypeConfiguration<Registration>
    {
        public void Configure(EntityTypeBuilder<Registration> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.StudentId)
                   .IsRequired()
                   .HasMaxLength(450);

            builder.HasOne(r => r.Student)
                   .WithMany(s => s.Registrations)
                   .HasForeignKey(r => r.StudentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Course)
                   .WithMany(c => c.Registrations)
                   .HasForeignKey(r => r.CourseId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.StudyYear)
                   .WithMany(sy => sy.Registrations)
                   .HasForeignKey(r => r.StudyYearId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Semester)
                   .WithMany(s => s.Registrations)
                   .HasForeignKey(r => r.SemesterId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}