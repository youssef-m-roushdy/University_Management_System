using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Infrastructure.Presistence.Data.Configurations
{
    // QuestionOptionConfiguration.cs
    public class QuestionOptionConfiguration : IEntityTypeConfiguration<QuestionOption>
    {
        public void Configure(EntityTypeBuilder<QuestionOption> builder)
        {
            builder.HasKey(o => o.Id);

            builder.HasOne(o => o.Question)
                   .WithMany(q => q.Options)
                   .HasForeignKey(o => o.QuestionId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(o => o.Text)
                   .IsRequired()
                   .HasMaxLength(500);
        }
    }
}