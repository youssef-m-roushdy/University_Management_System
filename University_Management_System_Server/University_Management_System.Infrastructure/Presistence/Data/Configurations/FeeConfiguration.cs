using University_Management_System.Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace University_Management_System.Infrastructure.Presistence.Data.Configurations
{
    public class FeeConfiguration : IEntityTypeConfiguration<Fee>
    {
        public void Configure(EntityTypeBuilder<Fee> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Amount)
                   .HasPrecision(18, 2);

            builder.HasOne(f => f.Department)
                   .WithMany(d => d.Fees)
                   .HasForeignKey(f => f.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.StudyYear)
                   .WithMany(sy => sy.Fees)
                   .HasForeignKey(f => f.StudyYearId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}