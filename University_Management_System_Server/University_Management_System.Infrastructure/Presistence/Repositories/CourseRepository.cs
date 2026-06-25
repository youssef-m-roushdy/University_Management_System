using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Enums;
using University_Management_System.Domain.Queries.CourseQueries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Queries;

namespace University_Management_System.Infrastructure.Presistence.Repositories
{
    public class CourseRepository : GenericRepository<Course, int>, ICourseRepository
    {
        public CourseRepository(UniversityDbContext dbContext) : base(dbContext)
        {
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY CRITERIA
        // ────────────────────────────────────────────────────────────────────────

        public async Task<Course?> GetCourseWithDetailsAsync(int courseId)
        {
            return await GetQueryable()
                .Include(c => c.Department)
                .Include(c => c.PrerequisiteFor)      // ✅ Using model property
                    .ThenInclude(p => p.PrerequisiteCourse)
                .Include(c => c.DependentCourses)     // ✅ Using model property
                    .ThenInclude(d => d.Course)
                .Include(c => c.CourseUpload)         // ✅ Using model property
                .FirstOrDefaultAsync(c => c.Id == courseId);
        }

        public async Task<Course?> GetCourseWithUploadsAsync(int courseId)
        {
            return await GetQueryable()
                .Include(c => c.CourseUpload)         // ✅ Using model property
                .FirstOrDefaultAsync(c => c.Id == courseId);
        }

        public async Task<Course?> GetCourseWithPrerequisitesAsync(int courseId)
        {
            return await GetQueryable()
                .Include(c => c.PrerequisiteFor)      // ✅ Using model property
                    .ThenInclude(p => p.PrerequisiteCourse)
                .FirstOrDefaultAsync(c => c.Id == courseId);
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET FILTERED
        // ────────────────────────────────────────────────────────────────────────

        public async Task<(IEnumerable<Course> Data, int TotalCount)> GetFilteredAsync(
            CourseFilterQueries query,
            CancellationToken cancellationToken = default)
        {
            var queryable = BuildBaseQuery();

            // ─── Apply Filters ────────────────────────────────────────────────
            if (query.DepartmentId.HasValue)
                queryable = queryable.Where(c => c.DepartmentId == query.DepartmentId.Value);

            if (query.Status.HasValue)
                queryable = queryable.Where(c => c.Status == query.Status.Value);

            if (!string.IsNullOrEmpty(query.Code))
                queryable = queryable.Where(c => c.Code.Contains(query.Code));

            if (!string.IsNullOrEmpty(query.Name))
                queryable = queryable.Where(c => c.Name.Contains(query.Name));

            if (query.MinCredits.HasValue)
                queryable = queryable.Where(c => c.Credits >= query.MinCredits.Value);

            if (query.MaxCredits.HasValue)
                queryable = queryable.Where(c => c.Credits <= query.MaxCredits.Value);

            if (query.HasPrerequisites.HasValue)
            {
                if (query.HasPrerequisites.Value)
                    queryable = queryable.Where(c => c.PrerequisiteFor.Any());  // ✅ Using model property
                else
                    queryable = queryable.Where(c => !c.PrerequisiteFor.Any()); // ✅ Using model property
            }


            // ─── Search Term ──────────────────────────────────────────────────
            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                var searchTerm = query.SearchTerm.ToLower();
                queryable = queryable.Where(c =>
                    c.Name.ToLower().Contains(searchTerm) ||
                    c.Code.ToLower().Contains(searchTerm));
            }

            var totalCount = await queryable.CountAsync(cancellationToken);

            // ─── Apply Sorting ────────────────────────────────────────────────
            queryable = query.SortBy?.ToLower() switch
            {
                "name" => query.SortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(c => c.Name)
                    : queryable.OrderByDescending(c => c.Name),
                "code" => query.SortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(c => c.Code)
                    : queryable.OrderByDescending(c => c.Code),
                "credits" => query.SortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(c => c.Credits)
                    : queryable.OrderByDescending(c => c.Credits),
                "status" => query.SortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(c => c.Status)
                    : queryable.OrderByDescending(c => c.Status),
                _ => queryable.OrderBy(c => c.Name)
            };

            // ─── Apply Pagination ─────────────────────────────────────────────
            var result = await queryable
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);

            return (result, totalCount);
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY DEPARTMENT
        // ────────────────────────────────────────────────────────────────────────

        public async Task<(IEnumerable<Course> Data, int TotalCount)> GetByDepartmentAsync(
            int departmentId,
            CourseDepartmentQueries query,
            CancellationToken cancellationToken = default)
        {
            var queryable = BuildBaseQuery()
                .Where(c => c.DepartmentId == departmentId);

            // ─── Apply Filters ────────────────────────────────────────────────
            if (query.Status.HasValue)
                queryable = queryable.Where(c => c.Status == query.Status.Value);

            if (!string.IsNullOrEmpty(query.Code))
                queryable = queryable.Where(c => c.Code.Contains(query.Code));

            if (!string.IsNullOrEmpty(query.Name))
                queryable = queryable.Where(c => c.Name.Contains(query.Name));

            if (query.MinCredits.HasValue)
                queryable = queryable.Where(c => c.Credits >= query.MinCredits.Value);

            if (query.MaxCredits.HasValue)
                queryable = queryable.Where(c => c.Credits <= query.MaxCredits.Value);

            if (query.HasPrerequisites.HasValue)
            {
                if (query.HasPrerequisites.Value)
                    queryable = queryable.Where(c => c.PrerequisiteFor.Any());  // ✅ Using model property
                else
                    queryable = queryable.Where(c => !c.PrerequisiteFor.Any()); // ✅ Using model property
            }

            // ─── Search Term ──────────────────────────────────────────────────
            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                var searchTerm = query.SearchTerm.ToLower();
                queryable = queryable.Where(c =>
                    c.Name.ToLower().Contains(searchTerm) ||
                    c.Code.ToLower().Contains(searchTerm));
            }

            var totalCount = await queryable.CountAsync(cancellationToken);

            // ─── Apply Sorting ────────────────────────────────────────────────
            queryable = query.SortBy?.ToLower() switch
            {
                "name" => query.SortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(c => c.Name)
                    : queryable.OrderByDescending(c => c.Name),
                "code" => query.SortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(c => c.Code)
                    : queryable.OrderByDescending(c => c.Code),
                "credits" => query.SortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(c => c.Credits)
                    : queryable.OrderByDescending(c => c.Credits),
                "status" => query.SortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(c => c.Status)
                    : queryable.OrderByDescending(c => c.Status),
                _ => queryable.OrderBy(c => c.Name)
            };

            // ─── Apply Pagination ─────────────────────────────────────────────
            var result = await queryable
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);

            return (result, totalCount);
        }

        // ────────────────────────────────────────────────────────────────────────
        // COURSE DEPENDENCIES & PREREQUISITES
        // ────────────────────────────────────────────────────────────────────────

        public async Task<IEnumerable<Course>> GetDependenciesAsync(int courseId)
        {
            return await GetQueryable()
                .Where(c => c.PrerequisiteFor.Any(p => p.PrerequisiteCourseId == courseId))  // ✅ Using model property
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetPrerequisitesAsync(int courseId)
        {
            var course = await GetCourseWithPrerequisitesAsync(courseId);
            return course?.PrerequisiteFor.Select(p => p.PrerequisiteCourse).ToList() ?? new List<Course>();  // ✅ Using model property
        }

        public async Task<IEnumerable<CoursePrerequisite>> GetPrerequisiteMappingsAsync()
        {
            return await _dbContext.CoursePrerequisites
                .Include(cp => cp.Course)
                .Include(cp => cp.PrerequisiteCourse)
                .ToListAsync();
        }

        public async Task<IEnumerable<CoursePrerequisite>> GetPrerequisiteMappingsForOpenCoursesAsync()
        {
            return await _dbContext.CoursePrerequisites
                .Include(cp => cp.Course)
                .Include(cp => cp.PrerequisiteCourse)
                .Where(cp => cp.Course.Status == CourseStatus.Opened)
                .ToListAsync();
        }

        // ────────────────────────────────────────────────────────────────────────
        // OPEN COURSES
        // ────────────────────────────────────────────────────────────────────────

        public async Task<IEnumerable<Course>> GetOpenCoursesAsync()
        {
            return await GetQueryable()
                .Where(c => c.Status == CourseStatus.Opened)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetAllPrerequisitesForOpenCoursesAsync()
        {
            var openCourseIds = await GetQueryable()
                .Where(c => c.Status == CourseStatus.Opened)
                .Select(c => c.Id)
                .ToListAsync();

            return await GetQueryable()
                .Where(c => c.PrerequisiteFor.Any(p => openCourseIds.Contains(p.CourseId)))  // ✅ Using model property
                .ToListAsync();
        }

        // ────────────────────────────────────────────────────────────────────────
        // PASSED COURSES
        // ────────────────────────────────────────────────────────────────────────

        public async Task<IEnumerable<Course>> GetPassedCoursesByStudentIdAsync(string studentId)
        {
            return await GetQueryable()
                .Where(c => c.Registrations.Any(r => r.StudentId == studentId && r.IsPassed))
                .ToListAsync();
        }

        // ────────────────────────────────────────────────────────────────────────
        // UPDATE OPERATIONS
        // ────────────────────────────────────────────────────────────────────────

        public async Task UpdateStatusAsync(int courseId, CourseStatus newStatus)
        {
            var course = await GetByIdAsync(courseId);
            if (course != null)
            {
                course.Status = newStatus;
                course.UpdatedAt = DateTime.UtcNow;
                await UpdateAsync(course);
                await SaveChangesAsync();
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // CHECK EXISTENCE
        // ────────────────────────────────────────────────────────────────────────

        public async Task<bool> CourseExistsAsync(int courseId)
        {
            return await GetQueryable().AnyAsync(c => c.Id == courseId);
        }

        public async Task<bool> CourseCodeExistsAsync(string code)
        {
            return await GetQueryable().AnyAsync(c => c.Code == code);
        }

        public async Task<bool> HasPrerequisitesAsync(int courseId)
        {
            return await GetQueryable()
                .AnyAsync(c => c.Id == courseId && c.PrerequisiteFor.Any());  // ✅ Using model property
        }

        public async Task<bool> HasDependenciesAsync(int courseId)
        {
            return await GetQueryable()
                .AnyAsync(c => c.PrerequisiteFor.Any(p => p.PrerequisiteCourseId == courseId));  // ✅ Using model property
        }

        // ────────────────────────────────────────────────────────────────────────
        // COUNTS
        // ────────────────────────────────────────────────────────────────────────

        public async Task<int> GetCourseCountByDepartmentAsync(int departmentId)
        {
            return await GetQueryable()
                .CountAsync(c => c.DepartmentId == departmentId);
        }

        public async Task<int> GetCourseCountByStatusAsync(CourseStatus status)
        {
            return await GetQueryable()
                .CountAsync(c => c.Status == status);
        }

        // ────────────────────────────────────────────────────────────────────────
        // BULK OPERATIONS
        // ────────────────────────────────────────────────────────────────────────

        public async Task AddRangeAsync(IEnumerable<Course> courses)
        {
            await _dbContext.Courses.AddRangeAsync(courses);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(IEnumerable<Course> courses)
        {
            _dbContext.Courses.UpdateRange(courses);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<Course> courses)
        {
            _dbContext.Courses.RemoveRange(courses);
            await _dbContext.SaveChangesAsync();
        }

        // ────────────────────────────────────────────────────────────────────────
        // PRIVATE HELPERS
        // ────────────────────────────────────────────────────────────────────────

        private IQueryable<Course> BuildBaseQuery()
        {
            return GetQueryable()
                .Include(c => c.Department)
                .Include(c => c.PrerequisiteFor)      // ✅ Using model property
                    .ThenInclude(p => p.PrerequisiteCourse)
                .Include(c => c.DependentCourses)     // ✅ Using model property
                    .ThenInclude(d => d.Course)
                .Include(c => c.CourseUpload)         // ✅ Using model property
                .AsNoTracking();
        }
    }
}