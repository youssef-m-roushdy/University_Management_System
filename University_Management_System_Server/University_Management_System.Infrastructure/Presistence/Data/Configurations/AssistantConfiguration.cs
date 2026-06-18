using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace University_Management_System.Infrastructure.Presistence.Data.Configurations
{
    public class AssistantConfiguration : IEntityTypeConfiguration<Assistant>
    {
        public void Configure(EntityTypeBuilder<Assistant> builder)
        {
            builder.HasKey(ia => ia.UserId);

            builder.HasOne(ia => ia.User)
                .WithOne(u => u.Assistant)
                .HasForeignKey<Assistant>(ia => ia.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ia => ia.Department)
                .WithMany()
                .HasForeignKey(ia => ia.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // supervisor — Restrict to avoid cascade conflict with User → InstructorAssistant
            builder.HasOne(ia => ia.Instructor)
                .WithMany(i => i.InstructorAssistants)
                .HasForeignKey(ia => ia.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(ia => ia.CourseAssistants)
                .WithOne(ca => ca.Assistant)
                .HasForeignKey(ca => ca.AssistantUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}