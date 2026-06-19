using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using University_Management_System.Domain.Entities.Identity;

namespace University_Management_System.Infrastructure.Presistence.Data.Configurations
{
    public class AssistantConfiguration : IEntityTypeConfiguration<Assistant>
    {
        public void Configure(EntityTypeBuilder<Assistant> builder)
        {
            builder.HasKey(ia => ia.Id);

            builder.HasOne(ia => ia.User)
                .WithOne(u => u.Assistant)
                .HasForeignKey<Assistant>(ia => ia.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ia => ia.Department)
                .WithMany()
                .HasForeignKey(ia => ia.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(ia => ia.CourseAssistants)
                .WithOne(ca => ca.Assistant)
                .HasForeignKey(ca => ca.AssistantUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(i => i.InstructorAssistants)
                .WithOne(ia => ia.Assistant)
                .HasForeignKey(ia => ia.InstructorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}