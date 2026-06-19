using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Infrastructure.Presistence.Data.Configurations
{
    // InstructorAssistantConfiguration.cs
public class InstructorAssistantConfiguration : IEntityTypeConfiguration<InstructorAssistant>
{
    public void Configure(EntityTypeBuilder<InstructorAssistant> builder)
    {
        builder.HasKey(x => x.Id);

        // Restrict on both — Instructor and Assistant each already cascade from User.
        // Using Cascade here too would create multiple cascade paths back to AspNetUsers
        // (SQL Server/PostgreSQL will reject this at migration time).
        builder.HasOne(x => x.Instructor)
               .WithMany(i => i.InstructorAssistants)
               .HasForeignKey(x => x.InstructorId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Assistant)
               .WithMany(a => a.InstructorAssistants)
               .HasForeignKey(x => x.AssistantId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Semester)
               .WithMany()
               .HasForeignKey(x => x.SemesterId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.StudyYear)
               .WithMany()
               .HasForeignKey(x => x.StudyYearId)
               .OnDelete(DeleteBehavior.Restrict);

        // prevents assigning the same assistant to the same instructor
        // twice in the same semester/year
        builder.HasIndex(x => new { x.InstructorId, x.AssistantId, x.SemesterId, x.StudyYearId })
               .IsUnique();
    }
}
}