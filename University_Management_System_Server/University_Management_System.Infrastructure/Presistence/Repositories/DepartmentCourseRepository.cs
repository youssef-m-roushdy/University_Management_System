using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Enums;
using University_Management_System.Domain.Queries.DepartmentCourseQueries;
using Microsoft.EntityFrameworkCore;
using University_Management_System.Domain.Queries;

namespace University_Management_System.Infrastructure.Presistence.Repositories
{
    public class DepartmentCourseRepository : GenericRepository<DepartmentCourse, int>, IDepartmentCourseRepository
    {
        public DepartmentCourseRepository(UniversityDbContext dbContext) : base(dbContext)
        {
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET COURSES BY DEPARTMENT (WITH PAGINATION)
        // ────────────────────────────────────────────────────────────────────────
        public async Task<IEnumerable<DepartmentCourse>> GetByDepartmentIdAsync(int departmentId)
        {
            return await GetQueryable()
                .Where(dc => dc.DepartmentId == departmentId)
                .ToListAsync();
        }
        public async Task<IEnumerable<DepartmentCourse>> GetByDepartmentIdWithDetailsAsync(int departmentId)
        {
            return await GetQueryable()
                .Include(dc => dc.Department)
                .Include(dc => dc.Course)
                .Where(dc => dc.DepartmentId == departmentId)
                .OrderBy(dc => dc.Course.Code)
                .ToListAsync();
        }
        public async Task<(IEnumerable<DepartmentCourse> Data, int TotalCount)> GetCoursesByDepartmentIdAsync(
            int departmentId,
            DepartmentCourseFilterQueries? filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = BuildBaseQuery()
                .Where(dc => dc.DepartmentId == departmentId)
                .AsQueryable();

            query = ApplyDepartmentFilters(query, filter);

            var totalCount = await query.CountAsync(cancellationToken);

            // Apply sorting
            query = filter?.SortBy?.ToLower() switch
            {
                "coursecode" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(dc => dc.Course.Code)
                    : query.OrderByDescending(dc => dc.Course.Code),
                "coursename" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(dc => dc.Course.Name)
                    : query.OrderByDescending(dc => dc.Course.Name),
                "role" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(dc => dc.Role)
                    : query.OrderByDescending(dc => dc.Role),
                "credits" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(dc => dc.Course.Credits)
                    : query.OrderByDescending(dc => dc.Course.Credits),
                "department" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(dc => dc.Department.Name)
                    : query.OrderByDescending(dc => dc.Department.Name),
                _ => query.OrderBy(dc => dc.Course.Code)
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
        // GET ALL DEPARTMENT COURSES (WITH PAGINATION - ADMIN ONLY)
        // ────────────────────────────────────────────────────────────────────────

        public async Task<(IEnumerable<DepartmentCourse> Data, int TotalCount)> GetAllDepartmentCoursesAsync(
            CourseFilterInDepartmentQueries? filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = BuildBaseQuery()
                .AsQueryable();

            query = ApplyGlobalFilters(query, filter);

            var totalCount = await query.CountAsync(cancellationToken);

            // Apply sorting (default by department then course code)
            query = query
                .OrderBy(dc => dc.Department.Name)
                .ThenBy(dc => dc.Course.Code);

            // Note: No pagination for admin view (can be added if needed)
            var result = await query.ToListAsync(cancellationToken);

            return (result, totalCount);
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY COURSE
        // ────────────────────────────────────────────────────────────────────────

        public async Task<IEnumerable<DepartmentCourse>> GetByCourseIdAsync(int courseId)
        {
            return await GetQueryable()
                .Where(dc => dc.CourseId == courseId)
                .ToListAsync();
        }

        public async Task<IEnumerable<DepartmentCourse>> GetByCourseIdWithDetailsAsync(int courseId)
        {
            return await GetQueryable()
                .Include(dc => dc.Department)
                .Include(dc => dc.Course)
                .Where(dc => dc.CourseId == courseId)
                .OrderBy(dc => dc.Department.Name)
                .ToListAsync();
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY ROLE
        // ────────────────────────────────────────────────────────────────────────

        public async Task<IEnumerable<DepartmentCourse>> GetByRoleAsync(CourseRole role)
        {
            return await GetQueryable()
                .Include(dc => dc.Course)
                .Include(dc => dc.Department)
                .Where(dc => dc.Role == role)
                .ToListAsync();
        }

        public async Task<IEnumerable<DepartmentCourse>> GetByDepartmentAndRoleAsync(int departmentId, CourseRole role)
        {
            return await GetQueryable()
                .Include(dc => dc.Course)
                .Where(dc => dc.DepartmentId == departmentId && dc.Role == role)
                .OrderBy(dc => dc.Course.Code)
                .ToListAsync();
        }

        // ────────────────────────────────────────────────────────────────────────
        // CHECK EXISTENCE
        // ────────────────────────────────────────────────────────────────────────

        public async Task<bool> ExistsAsync(int departmentId, int courseId)
        {
            return await GetQueryable()
                .AnyAsync(dc => dc.DepartmentId == departmentId && dc.CourseId == courseId);
        }

        public async Task<bool> ExistsByDepartmentAndRoleAsync(int departmentId, CourseRole role)
        {
            return await GetQueryable()
                .AnyAsync(dc => dc.DepartmentId == departmentId && dc.Role == role);
        }

        // ────────────────────────────────────────────────────────────────────────
        // DELETE OPERATIONS
        // ────────────────────────────────────────────────────────────────────────

        public async Task DeleteByDepartmentIdAsync(int departmentId)
        {
            var mappings = await GetByCourseIdAsync(departmentId);
            if (mappings.Any())
            {
                _dbContext.DepartmentCourses.RemoveRange(mappings);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteByCourseIdAsync(int courseId)
        {
            var mappings = await GetByCourseIdAsync(courseId);
            if (mappings.Any())
            {
                _dbContext.DepartmentCourses.RemoveRange(mappings);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteRangeAsync(IEnumerable<DepartmentCourse> departmentCourses)
        {
            if (departmentCourses.Any())
            {
                _dbContext.DepartmentCourses.RemoveRange(departmentCourses);
                await _dbContext.SaveChangesAsync();
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // PRIVATE HELPERS
        // ────────────────────────────────────────────────────────────────────────

        private IQueryable<DepartmentCourse> BuildBaseQuery()
        {
            return GetQueryable()
                .Include(dc => dc.Department)
                .Include(dc => dc.Course)
                .AsNoTracking();
        }

        private IQueryable<DepartmentCourse> ApplyDepartmentFilters(
            IQueryable<DepartmentCourse> query,
            DepartmentCourseFilterQueries? filter)
        {
            if (filter == null) return query;

            if (filter.Role.HasValue)
                query = query.Where(dc => dc.Role == filter.Role.Value);

            if (!string.IsNullOrEmpty(filter.CourseName))
                query = query.Where(dc => dc.Course.Name.Contains(filter.CourseName));

            if (!string.IsNullOrEmpty(filter.CourseCode))
                query = query.Where(dc => dc.Course.Code.Contains(filter.CourseCode));

            if (filter.MinCredits.HasValue)
                query = query.Where(dc => dc.Course.Credits >= filter.MinCredits.Value);

            if (filter.MaxCredits.HasValue)
                query = query.Where(dc => dc.Course.Credits <= filter.MaxCredits.Value);

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                var searchTerm = filter.SearchTerm.ToLower();
                query = query.Where(dc =>
                    dc.Course.Code.ToLower().Contains(searchTerm) ||
                    dc.Course.Name.ToLower().Contains(searchTerm) ||
                    dc.Department.Name.ToLower().Contains(searchTerm));
            }

            return query;
        }

        private IQueryable<DepartmentCourse> ApplyGlobalFilters(
            IQueryable<DepartmentCourse> query,
            CourseFilterInDepartmentQueries? filter)
        {
            if (filter == null) return query;

            if (filter.Role.HasValue)
                query = query.Where(dc => dc.Role == filter.Role.Value);

            if (!string.IsNullOrEmpty(filter.CourseName))
                query = query.Where(dc => dc.Course.Name.Contains(filter.CourseName));

            if (!string.IsNullOrEmpty(filter.CourseCode))
                query = query.Where(dc => dc.Course.Code.Contains(filter.CourseCode));

            if (filter.MinCredits.HasValue)
                query = query.Where(dc => dc.Course.Credits >= filter.MinCredits.Value);

            if (filter.MaxCredits.HasValue)
                query = query.Where(dc => dc.Course.Credits <= filter.MaxCredits.Value);

            return query;
        }
    }
}