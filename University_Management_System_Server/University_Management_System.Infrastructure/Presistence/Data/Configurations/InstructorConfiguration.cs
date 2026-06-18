using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace University_Management_System.Infrastructure.Presistence.Data.Configurations
{
    public class InstructorConfiguration: IEntityTypeConfiguration<Instructor>
    {
            public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.HasKey(i => i.UserId);

            builder.HasOne(i => i.User)
                .WithOne(u => u.Instructor)
                .HasForeignKey<Instructor>(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(i => i.Department)
                .WithMany()
                .HasForeignKey(i => i.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // assistants supervised by this instructor
            builder.HasMany(i => i.InstructorAssistants)
                .WithOne(ia => ia.Instructor)
                .HasForeignKey(ia => ia.InstructorId)
                .OnDelete(DeleteBehavior.Restrict); // don't delete TAs if instructor is deleted
        }
    }
}