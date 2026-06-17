using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace University_Management_System.Infrastructure.Presistence.Repositories
{
    public class StudyYearRepository : GenericRepository<StudyYear, int>, IStudyYearRepository
    {
        public StudyYearRepository(UniversityDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<StudyYear?> GetCurrentStudyYearAsync()
        {
            return await _dbContext.StudyYears
                .Where(sy => sy.IsCurrent)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsCurrentStudyYearAsync(int studyYearId)
        {
            return await _dbContext.StudyYears
                .AnyAsync(sy => sy.Id == studyYearId && sy.IsCurrent);
        }
    }
}
