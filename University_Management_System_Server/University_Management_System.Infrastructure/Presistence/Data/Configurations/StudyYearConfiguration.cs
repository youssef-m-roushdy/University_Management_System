using University_Management_System.Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace University_Management_System.Infrastructure.Presistence.Data.Configurations
{
    public class StudyYearConfiguration : IEntityTypeConfiguration<StudyYear>
    {
        public void Configure(EntityTypeBuilder<StudyYear> builder)
        {
            builder.HasKey(s => s.Id);
        }
    }
}