using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace University_Management_System.Infrastructure.Presistence.Repositories
{
    public class StudentStudyYearRepository : GenericRepository<StudentStudyYear, int>, IStudentStudyYearRepository
    {
        public StudentStudyYearRepository(UniversityDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<StudentStudyYear>> GetByStudentIdAsync(string StudentId)
        {
            return await _dbContext.StudentStudyYears
                .Include(usy => usy.StudyYear)
                .Where(usy => usy.StudentId == StudentId)
                .OrderBy(usy => usy.StudyYear.StartYear)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<StudentStudyYear>> GetStudyYearsByStudentIdAsync(string StudentId)
        {
            // get Student study year and study year details for a specific Student
            return await _dbContext.StudentStudyYears
                .Where(usy => usy.StudentId == StudentId)
                .Include(usy => usy.StudyYear)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddRangeAsync(IEnumerable<StudentStudyYear> StudentStudyYears)
        {
            await _dbContext.StudentStudyYears.AddRangeAsync(StudentStudyYears);
        }

        public async Task<StudentStudyYear?> GetCurrentByStudentIdAsync(string StudentId)
        {
            return await _dbContext.StudentStudyYears
                .Include(usy => usy.StudyYear)
                .Where(usy => usy.StudentId == StudentId && usy.StudyYear.IsCurrent)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<StudentStudyYear?> GetByStudentAndStudyYearAsync(string StudentId, int studyYearId)
        {
            return await _dbContext.StudentStudyYears
                .Include(usy => usy.StudyYear)
                .Where(usy => usy.StudentId == StudentId && usy.StudyYearId == studyYearId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<StudentStudyYear>> GetByStudyYearIdAsync(int studyYearId)
        {
            return await _dbContext.StudentStudyYears
                .Include(usy => usy.StudyYear)
                .Where(usy => usy.StudyYearId == studyYearId)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
