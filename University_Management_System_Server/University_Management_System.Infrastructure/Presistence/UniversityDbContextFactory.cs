using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace University_Management_System.Infrastructure.Presistence
{
    public class UniversityDbContextFactory : IDesignTimeDbContextFactory<UniversityDbContext>
    {
        public UniversityDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UniversityDbContext>();
            optionsBuilder.UseSqlServer("Server=localhost;Database=universitydb;User Id=sa;Password=Yossef_2004;TrustServerCertificate=True;");

            return new UniversityDbContext(optionsBuilder.Options);
        }
    }
}
