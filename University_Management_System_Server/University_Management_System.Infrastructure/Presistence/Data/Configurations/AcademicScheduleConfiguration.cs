using University_Management_System.Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace University_Management_System.Infrastructure.Presistence.Data.Configurations
{
    public class AcademicScheduleConfiguration : IEntityTypeConfiguration<AcademicSchedule>
    {
        public void Configure(EntityTypeBuilder<AcademicSchedule> builder)
        {
            builder.HasKey(a => a.Id);

            builder.HasOne(a => a.Department)
                   .WithMany(d => d.AcademicSchedules)
                   .HasForeignKey(a => a.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Map Admin → AdminId so EF does not generate a shadow UserId column
            builder.HasOne(a => a.Admin)
                   .WithMany(u => u.AcademicSchedules)
                   .HasForeignKey(a => a.AdminId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(a => a.AdminId)
                   .IsRequired()
                   .HasMaxLength(450);

            builder.Property(a => a.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(a => a.Url)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.HasOne(a => a.Semester)
                   .WithMany(s => s.AcademicSchedules)
                   .HasForeignKey(a => a.SemesterId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.StudyYear)
                   .WithMany(sy => sy.AcademicSchedules)
                   .HasForeignKey(a => a.StudyYearId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}