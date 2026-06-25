using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace University_Management_System.Infrastructure.Presistence.Repositories
{
    public class CoursePrerequisiteRepository : GenericRepository<CoursePrerequisite, int>, ICoursePrerequisiteRepository
    {
        public CoursePrerequisiteRepository(UniversityDbContext dbContext) : base(dbContext)
        {
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY COURSE
        // ────────────────────────────────────────────────────────────────────────

        public async Task<IEnumerable<CoursePrerequisite>> GetByCourseIdAsync(int courseId)
        {
            return await GetQueryable()
                .Where(cp => cp.CourseId == courseId)
                .ToListAsync();
        }

        public async Task<IEnumerable<CoursePrerequisite>> GetByPrerequisiteCourseIdAsync(int prerequisiteCourseId)
        {
            return await GetQueryable()
                .Where(cp => cp.PrerequisiteCourseId == prerequisiteCourseId)
                .ToListAsync();
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET WITH DETAILS
        // ────────────────────────────────────────────────────────────────────────

        public async Task<IEnumerable<CoursePrerequisite>> GetByCourseIdWithDetailsAsync(int courseId)
        {
            return await GetQueryable()
                .Include(cp => cp.Course)
                .Include(cp => cp.PrerequisiteCourse)
                .Where(cp => cp.CourseId == courseId)
                .ToListAsync();
        }

        public async Task<IEnumerable<CoursePrerequisite>> GetByPrerequisiteCourseIdWithDetailsAsync(int prerequisiteCourseId)
        {
            return await GetQueryable()
                .Include(cp => cp.Course)
                .Include(cp => cp.PrerequisiteCourse)
                .Where(cp => cp.PrerequisiteCourseId == prerequisiteCourseId)
                .ToListAsync();
        }

        // ────────────────────────────────────────────────────────────────────────
        // CHECK EXISTENCE
        // ────────────────────────────────────────────────────────────────────────

        public async Task<bool> ExistsAsync(int courseId, int prerequisiteCourseId)
        {
            return await GetQueryable()
                .AnyAsync(cp => cp.CourseId == courseId && cp.PrerequisiteCourseId == prerequisiteCourseId);
        }

        public async Task<bool> HasPrerequisitesAsync(int courseId)
        {
            return await GetQueryable()
                .AnyAsync(cp => cp.CourseId == courseId);
        }

        public async Task<bool> HasDependenciesAsync(int courseId)
        {
            return await GetQueryable()
                .AnyAsync(cp => cp.PrerequisiteCourseId == courseId);
        }

        // ────────────────────────────────────────────────────────────────────────
        // DELETE OPERATIONS
        // ────────────────────────────────────────────────────────────────────────

        public async Task DeleteByCourseIdAsync(int courseId)
        {
            var prerequisites = await GetByCourseIdAsync(courseId);
            if (prerequisites.Any())
            {
                _dbContext.CoursePrerequisites.RemoveRange(prerequisites);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteRangeAsync(IEnumerable<CoursePrerequisite> prerequisites)
        {
            if (prerequisites.Any())
            {
                _dbContext.CoursePrerequisites.RemoveRange(prerequisites);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}