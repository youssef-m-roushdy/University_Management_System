using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace University_Management_System.Infrastructure.Presistence.Data.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.UserId);

            builder.HasOne(s => s.User)
                .WithOne(u => u.Student)
                .HasForeignKey<Student>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.Department)
                .WithMany()
                .HasForeignKey(s => s.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(s => s.Specialization)
                .WithMany()
                .HasForeignKey(s => s.SpecializationId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(s => s.AcademicCode)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.TotalGPA)
                .HasPrecision(4, 2); // e.g. 3.75

            builder.Property(s => s.Level)
                .HasConversion<string>()
                .HasMaxLength(50);
        }
    }
}