using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace University_Management_System.Infrastructure.Presistence.Data.Configurations
{
    public class SpecializationCourseConfiguration : IEntityTypeConfiguration<SpecializationCourse>
    {
        public void Configure(EntityTypeBuilder<SpecializationCourse> builder)
        {
            builder.HasKey(sc => sc.Id);

            // Unique constraint: a course can appear only once per specialization
            builder.HasIndex(sc => new { sc.SpecializationId, sc.CourseId })
                   .IsUnique();

            // Relationships
            builder.HasOne(sc => sc.Specialization)
                   .WithMany(s => s.SpecializationCourses)
                   .HasForeignKey(sc => sc.SpecializationId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(sc => sc.Course)
                   .WithMany(c => c.SpecializationCourses)
                   .HasForeignKey(sc => sc.CourseId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Enum conversion (store as string)
            builder.Property(sc => sc.Role)
                   .HasConversion<string>()
                   .IsRequired();
        }
    }
}