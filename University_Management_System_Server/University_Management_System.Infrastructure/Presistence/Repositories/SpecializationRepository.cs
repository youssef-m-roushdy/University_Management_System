using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Enums;
using University_Management_System.Infrastructure.Presistence;
using Microsoft.EntityFrameworkCore;

namespace University_Management_System.Infrastructure.Presistence.Repositories
{
    public class SpecializationRepository : GenericRepository<Specialization, int>, ISpecializationRepository
    {
        public SpecializationRepository(UniversityDbContext dbContext) : base(dbContext)
        {
        }

        // ── Queries ────────────────────────────────────────────────────────────

        public async Task<IEnumerable<Specialization>> GetByDepartmentIdAsync(int departmentId)
        {
            return await _dbContext.Specializations
                .AsNoTracking()
                .Where(s => s.DepartmentId == departmentId)
                .Include(s => s.Department)
                .ToListAsync();
        }

        public async Task<Specialization?> GetWithCoursesAsync(int specializationId)
        {
            return await _dbContext.Specializations
                .AsNoTracking()
                .Where(s => s.Id == specializationId)
                .Include(s => s.Department)
                .Include(s => s.SpecializationCourses)
                    .ThenInclude(sc => sc.Course)
                .FirstOrDefaultAsync();
        }

        public async Task<Specialization?> GetWithDepartmentAsync(int specializationId)
        {
            return await _dbContext.Specializations
                .AsNoTracking()
                .Where(s => s.Id == specializationId)
                .Include(s => s.Department)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<SpecializationCourse>> GetSpecializationCoursesAsync(
            int specializationId,
            SpecializationCourseRole? role = null)
        {
            var query = _dbContext.SpecializationCourses
                .AsNoTracking()
                .Where(sc => sc.SpecializationId == specializationId)
                .Include(sc => sc.Course)
                    .ThenInclude(c => c.Department)
                .AsQueryable();

            if (role.HasValue)
                query = query.Where(sc => sc.Role == role.Value);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<User>> GetStudentsAsync(int specializationId)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .Where(u => u.SpecializationId == specializationId)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(string name, int departmentId)
        {
            return await _dbContext.Specializations
                .AnyAsync(s => s.Name == name && s.DepartmentId == departmentId);
        }

        // ── SpecializationCourse link management ───────────────────────────────

        public async Task<SpecializationCourse?> GetSpecializationCourseAsync(
            int specializationId,
            int courseId)
        {
            return await _dbContext.SpecializationCourses
                .FirstOrDefaultAsync(sc =>
                    sc.SpecializationId == specializationId &&
                    sc.CourseId == courseId);
        }

        public async Task AddCourseAsync(SpecializationCourse specializationCourse)
        {
            await _dbContext.SpecializationCourses.AddAsync(specializationCourse);
        }

        public Task RemoveCourseAsync(SpecializationCourse specializationCourse)
        {
            _dbContext.SpecializationCourses.Remove(specializationCourse);
            return Task.CompletedTask;
        }

        public Task UpdateCourseRoleAsync(SpecializationCourse specializationCourse)
        {
            _dbContext.SpecializationCourses.Update(specializationCourse);
            return Task.CompletedTask;
        }
    }
}