using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Enums;
using University_Management_System.Domain.Queries.SpecializationCourseQueries;
using Microsoft.EntityFrameworkCore;
using University_Management_System.Domain.Queries;

namespace University_Management_System.Infrastructure.Presistence.Repositories
{
    public class SpecializationCourseRepository : GenericRepository<SpecializationCourse, int>, ISpecializationCourseRepository
    {
        public SpecializationCourseRepository(UniversityDbContext dbContext) : base(dbContext)
        {
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY SPECIALIZATION
        // ────────────────────────────────────────────────────────────────────────

        public async Task<IEnumerable<SpecializationCourse>> GetBySpecializationIdAsync(int specializationId)
        {
            return await GetQueryable()
                .Where(sc => sc.SpecializationId == specializationId)
                .OrderBy(sc => sc.Course.Code)
                .ToListAsync();
        }

        public async Task<IEnumerable<SpecializationCourse>> GetBySpecializationIdWithDetailsAsync(int specializationId)
        {
            return await GetQueryable()
                .Include(sc => sc.Specialization)
                    .ThenInclude(s => s.Department)
                .Include(sc => sc.Course)
                    .ThenInclude(c => c.Department)
                .Include(sc => sc.Course)
                    .ThenInclude(c => c.PrerequisiteFor)
                        .ThenInclude(p => p.PrerequisiteCourse)
                .Where(sc => sc.SpecializationId == specializationId)
                .OrderBy(sc => sc.Course.Code)
                .ToListAsync();
        }

        public async Task<(IEnumerable<SpecializationCourse> Data, int TotalCount)> GetBySpecializationIdAsync(
            int specializationId,
            CourseFilterInSpecailizationQueries? filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = BuildBaseQuery()
                .Where(sc => sc.SpecializationId == specializationId)
                .AsQueryable();

            query = ApplyCourseFilterInSpecializationFilters(query, filter);

            var totalCount = await query.CountAsync(cancellationToken);

            // Apply sorting
            query = filter?.SortBy?.ToLower() switch
            {
                "coursecode" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(sc => sc.Course.Code)
                    : query.OrderByDescending(sc => sc.Course.Code),
                "coursename" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(sc => sc.Course.Name)
                    : query.OrderByDescending(sc => sc.Course.Name),
                "credits" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(sc => sc.Course.Credits)
                    : query.OrderByDescending(sc => sc.Course.Credits),
                "specialization" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(sc => sc.Specialization.Name)
                    : query.OrderByDescending(sc => sc.Specialization.Name),
                _ => query.OrderBy(sc => sc.Course.Code)
            };

            // Apply pagination
            var pageNumber = filter?.PageNumber ?? 1;
            var pageSize = filter?.PageSize ?? 10;

            var result = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (result, totalCount);
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY COURSE
        // ────────────────────────────────────────────────────────────────────────

        public async Task<IEnumerable<SpecializationCourse>> GetByCourseIdAsync(int courseId)
        {
            return await GetQueryable()
                .Where(sc => sc.CourseId == courseId)
                .OrderBy(sc => sc.Specialization.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<SpecializationCourse>> GetByCourseIdWithDetailsAsync(int courseId)
        {
            return await GetQueryable()
                .Include(sc => sc.Specialization)
                    .ThenInclude(s => s.Department)
                .Include(sc => sc.Course)
                    .ThenInclude(c => c.Department)
                .Where(sc => sc.CourseId == courseId)
                .OrderBy(sc => sc.Specialization.Name)
                .ToListAsync();
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY ROLE
        // ────────────────────────────────────────────────────────────────────────

        public async Task<IEnumerable<SpecializationCourse>> GetByRoleAsync(SpecializationCourseRole role)
        {
            return await GetQueryable()
                .Include(sc => sc.Course)
                .Include(sc => sc.Specialization)
                .Where(sc => sc.Role == role)
                .OrderBy(sc => sc.Specialization.Name)
                .ThenBy(sc => sc.Course.Code)
                .ToListAsync();
        }

        public async Task<IEnumerable<SpecializationCourse>> GetBySpecializationAndRoleAsync(
            int specializationId, 
            SpecializationCourseRole role)
        {
            return await GetQueryable()
                .Include(sc => sc.Course)
                .Where(sc => sc.SpecializationId == specializationId && sc.Role == role)
                .OrderBy(sc => sc.Course.Code)
                .ToListAsync();
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET ALL WITH FILTERS
        // ────────────────────────────────────────────────────────────────────────

        public async Task<(IEnumerable<SpecializationCourse> Data, int TotalCount)> GetAllFilteredAsync(
            SpecializationCourseFilterQueries? filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = BuildBaseQuery()
                .AsQueryable();

            query = ApplyGlobalFilters(query, filter);

            var totalCount = await query.CountAsync(cancellationToken);

            // Apply sorting
            query = filter?.SortBy?.ToLower() switch
            {
                "coursecode" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(sc => sc.Course.Code)
                    : query.OrderByDescending(sc => sc.Course.Code),
                "coursename" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(sc => sc.Course.Name)
                    : query.OrderByDescending(sc => sc.Course.Name),
                "role" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(sc => sc.Role)
                    : query.OrderByDescending(sc => sc.Role),
                "specialization" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(sc => sc.Specialization.Name)
                    : query.OrderByDescending(sc => sc.Specialization.Name),
                _ => query.OrderBy(sc => sc.Course.Code)
            };

            // Apply pagination
            var pageNumber = filter?.PageNumber ?? 1;
            var pageSize = filter?.PageSize ?? 10;

            var result = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (result, totalCount);
        }

        // ────────────────────────────────────────────────────────────────────────
        // CHECK EXISTENCE
        // ────────────────────────────────────────────────────────────────────────

        public async Task<bool> ExistsAsync(int specializationId, int courseId)
        {
            return await GetQueryable()
                .AnyAsync(sc => sc.SpecializationId == specializationId && sc.CourseId == courseId);
        }

        public async Task<bool> ExistsBySpecializationAndRoleAsync(int specializationId, SpecializationCourseRole role)
        {
            return await GetQueryable()
                .AnyAsync(sc => sc.SpecializationId == specializationId && sc.Role == role);
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await GetQueryable()
                .AnyAsync(sc => sc.Id == id);
        }

        // ────────────────────────────────────────────────────────────────────────
        // COUNTS
        // ────────────────────────────────────────────────────────────────────────

        public async Task<int> GetCountBySpecializationAsync(int specializationId)
        {
            return await GetQueryable()
                .CountAsync(sc => sc.SpecializationId == specializationId);
        }

        public async Task<int> GetCountByCourseAsync(int courseId)
        {
            return await GetQueryable()
                .CountAsync(sc => sc.CourseId == courseId);
        }

        public async Task<int> GetCountByRoleAsync(SpecializationCourseRole role)
        {
            return await GetQueryable()
                .CountAsync(sc => sc.Role == role);
        }

        // ────────────────────────────────────────────────────────────────────────
        // BULK OPERATIONS
        // ────────────────────────────────────────────────────────────────────────

        public async Task AddRangeAsync(IEnumerable<SpecializationCourse> specializationCourses)
        {
            await _dbContext.SpecializationCourses.AddRangeAsync(specializationCourses);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(IEnumerable<SpecializationCourse> specializationCourses)
        {
            _dbContext.SpecializationCourses.UpdateRange(specializationCourses);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<SpecializationCourse> specializationCourses)
        {
            _dbContext.SpecializationCourses.RemoveRange(specializationCourses);
            await _dbContext.SaveChangesAsync();
        }

        // ────────────────────────────────────────────────────────────────────────
        // PRIVATE HELPERS
        // ────────────────────────────────────────────────────────────────────────

        private IQueryable<SpecializationCourse> BuildBaseQuery()
        {
            return GetQueryable()
                .Include(sc => sc.Specialization)
                    .ThenInclude(s => s.Department)
                .Include(sc => sc.Course)
                    .ThenInclude(c => c.Department)
                .AsNoTracking();
        }

        private IQueryable<SpecializationCourse> ApplyCourseFilterInSpecializationFilters(
            IQueryable<SpecializationCourse> query,
            CourseFilterInSpecailizationQueries? filter)
        {
            if (filter == null) return query;

            if (filter.CourseId.HasValue)
                query = query.Where(sc => sc.CourseId == filter.CourseId.Value);

            if (!string.IsNullOrEmpty(filter.CourseName))
                query = query.Where(sc => sc.Course.Name.Contains(filter.CourseName));

            if (!string.IsNullOrEmpty(filter.CourseCode))
                query = query.Where(sc => sc.Course.Code.Contains(filter.CourseCode));

            if (filter.MinCredits.HasValue)
                query = query.Where(sc => sc.Course.Credits >= filter.MinCredits.Value);

            if (filter.MaxCredits.HasValue)
                query = query.Where(sc => sc.Course.Credits <= filter.MaxCredits.Value);

            if (filter.HasPrerequisites.HasValue)
            {
                if (filter.HasPrerequisites.Value)
                    query = query.Where(sc => sc.Course.PrerequisiteFor.Any());
                else
                    query = query.Where(sc => !sc.Course.PrerequisiteFor.Any());
            }

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                var searchTerm = filter.SearchTerm.ToLower();
                query = query.Where(sc =>
                    sc.Course.Code.ToLower().Contains(searchTerm) ||
                    sc.Course.Name.ToLower().Contains(searchTerm) ||
                    sc.Specialization.Name.ToLower().Contains(searchTerm));
            }

            return query;
        }

        private IQueryable<SpecializationCourse> ApplyGlobalFilters(
            IQueryable<SpecializationCourse> query,
            SpecializationCourseFilterQueries? filter)
        {
            if (filter == null) return query;

            if (filter.SpecializationId.HasValue)
                query = query.Where(sc => sc.SpecializationId == filter.SpecializationId.Value);

            if (filter.CourseId.HasValue)
                query = query.Where(sc => sc.CourseId == filter.CourseId.Value);

            if (filter.Role.HasValue)
                query = query.Where(sc => sc.Role == filter.Role.Value);

            if (!string.IsNullOrEmpty(filter.CourseName))
                query = query.Where(sc => sc.Course.Name.Contains(filter.CourseName));

            if (!string.IsNullOrEmpty(filter.CourseCode))
                query = query.Where(sc => sc.Course.Code.Contains(filter.CourseCode));

            if (filter.MinCredits.HasValue)
                query = query.Where(sc => sc.Course.Credits >= filter.MinCredits.Value);

            if (filter.MaxCredits.HasValue)
                query = query.Where(sc => sc.Course.Credits <= filter.MaxCredits.Value);

            if (filter.HasPrerequisites.HasValue)
            {
                if (filter.HasPrerequisites.Value)
                    query = query.Where(sc => sc.Course.PrerequisiteFor.Any());
                else
                    query = query.Where(sc => !sc.Course.PrerequisiteFor.Any());
            }

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                var searchTerm = filter.SearchTerm.ToLower();
                query = query.Where(sc =>
                    sc.Course.Code.ToLower().Contains(searchTerm) ||
                    sc.Course.Name.ToLower().Contains(searchTerm) ||
                    sc.Specialization.Name.ToLower().Contains(searchTerm));
            }

            return query;
        }
    }
}