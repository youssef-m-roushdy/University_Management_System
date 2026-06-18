using University_Management_System.Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace University_Management_System.Infrastructure.Presistence.Data.Configurations
{
    public class CourseUploadsConfiguration : IEntityTypeConfiguration<CourseUpload>
    {
        public void Configure(EntityTypeBuilder<CourseUpload> builder)
        {
            builder.HasKey(cu => cu.Id);

            builder.HasOne(cu => cu.Course)
                   .WithMany(c => c.CourseUpload)
                   .HasForeignKey(cu => cu.CourseId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(cu => cu.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(cu => cu.Type)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(cu => cu.FileId)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(cu => cu.Url)
                   .IsRequired()
                   .HasMaxLength(500);
        }
    }
}
