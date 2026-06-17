using University_Management_System.Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace University_Management_System.Infrastructure.Presistence.Data.Configurations
{
    public class SemesterConfiguration : IEntityTypeConfiguration<Semester>
    {
        public void Configure(EntityTypeBuilder<Semester> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.IsActive)
                   .HasDefaultValue(false);

            builder.HasOne(s => s.StudyYear)
                   .WithMany(sy => sy.Semesters)
                   .HasForeignKey(s => s.StudyYearId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
