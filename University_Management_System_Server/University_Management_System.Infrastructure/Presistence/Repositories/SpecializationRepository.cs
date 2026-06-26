using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Queries.SpecializationQueries;
using Microsoft.EntityFrameworkCore;
using University_Management_System.Domain.Queries;

namespace University_Management_System.Infrastructure.Presistence.Repositories
{
    public class SpecializationRepository : GenericRepository<Specialization, int>, ISpecializationRepository
    {
        public SpecializationRepository(UniversityDbContext dbContext) : base(dbContext)
        {
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY DEPARTMENT (WITHOUT PAGINATION)
        // ────────────────────────────────────────────────────────────────────────

        public async Task<IEnumerable<Specialization>> GetByDepartmentIdAsync(int departmentId)
        {
            return await GetQueryable()
                .Where(s => s.DepartmentId == departmentId)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Specialization>> GetByDepartmentIdWithDetailsAsync(int departmentId)
        {
            return await GetQueryable()
                .Include(s => s.Department)
                .Include(s => s.SpecializationCourses)
                    .ThenInclude(sc => sc.Course)
                .Include(s => s.Students)
                    .ThenInclude(st => st.User)
                .Where(s => s.DepartmentId == departmentId)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY DEPARTMENT (WITH PAGINATION)
        // ────────────────────────────────────────────────────────────────────────

        public async Task<(IEnumerable<Specialization> Data, int TotalCount)> GetByDepartmentIdAsync(
            int departmentId,
            SpecializationDepartmentQueries? filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = GetQueryable()
                .Include(s => s.Department)
                .Include(s => s.Students)
                .Include(s => s.SpecializationCourses)
                .Where(s => s.DepartmentId == departmentId)
                .AsQueryable();

            query = ApplyDepartmentFilters(query, filter);

            var totalCount = await query.CountAsync(cancellationToken);

            // Apply sorting
            query = filter?.SortBy?.ToLower() switch
            {
                "name" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(s => s.Name)
                    : query.OrderByDescending(s => s.Name),
                "studentcount" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(s => s.Students.Count)
                    : query.OrderByDescending(s => s.Students.Count),
                "coursecount" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(s => s.SpecializationCourses.Count)
                    : query.OrderByDescending(s => s.SpecializationCourses.Count),
                _ => query.OrderBy(s => s.Name)
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
        // GET ALL WITH FILTERS
        // ────────────────────────────────────────────────────────────────────────

        public async Task<(IEnumerable<Specialization> Data, int TotalCount)> GetAllFilteredAsync(
            SpecializationFilterQueries? filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = GetQueryable()
                .Include(s => s.Department)
                .Include(s => s.Students)
                .Include(s => s.SpecializationCourses)
                .AsQueryable();

            query = ApplyGlobalFilters(query, filter);

            var totalCount = await query.CountAsync(cancellationToken);

            // Apply sorting
            query = filter?.SortBy?.ToLower() switch
            {
                "name" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(s => s.Name)
                    : query.OrderByDescending(s => s.Name),
                "department" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(s => s.Department.Name)
                    : query.OrderByDescending(s => s.Department.Name),
                "studentcount" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(s => s.Students.Count)
                    : query.OrderByDescending(s => s.Students.Count),
                "coursecount" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(s => s.SpecializationCourses.Count)
                    : query.OrderByDescending(s => s.SpecializationCourses.Count),
                _ => query.OrderBy(s => s.Name)
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
        // GET BY NAME
        // ────────────────────────────────────────────────────────────────────────

        public async Task<Specialization?> GetByNameAsync(string name)
        {
            return await GetQueryable()
                .FirstOrDefaultAsync(s => s.Name == name);
        }

        public async Task<Specialization?> GetByNameWithDetailsAsync(string name)
        {
            return await GetQueryable()
                .Include(s => s.Department)
                .Include(s => s.SpecializationCourses)
                    .ThenInclude(sc => sc.Course)
                .Include(s => s.Students)
                    .ThenInclude(st => st.User)
                .FirstOrDefaultAsync(s => s.Name == name);
        }

        // ────────────────────────────────────────────────────────────────────────
        // CHECK EXISTENCE
        // ────────────────────────────────────────────────────────────────────────

        public async Task<bool> ExistsAsync(string name)
        {
            return await GetQueryable()
                .AnyAsync(s => s.Name == name);
        }

        public async Task<bool> ExistsByNameAndDepartmentAsync(string name, int departmentId)
        {
            return await GetQueryable()
                .AnyAsync(s => s.Name == name && s.DepartmentId == departmentId);
        }

        public async Task<bool> ExistsByIdAsync(int specializationId)
        {
            return await GetQueryable()
                .AnyAsync(s => s.Id == specializationId);
        }

        // ────────────────────────────────────────────────────────────────────────
        // COUNTS
        // ────────────────────────────────────────────────────────────────────────

        public async Task<int> GetCountByDepartmentAsync(int departmentId)
        {
            return await GetQueryable()
                .CountAsync(s => s.DepartmentId == departmentId);
        }

        public async Task<int> GetStudentCountAsync(int specializationId)
        {
            return await GetQueryable()
                .Where(s => s.Id == specializationId)
                .SelectMany(s => s.Students)
                .CountAsync();
        }

        public async Task<int> GetCourseCountAsync(int specializationId)
        {
            return await GetQueryable()
                .Where(s => s.Id == specializationId)
                .SelectMany(s => s.SpecializationCourses)
                .CountAsync();
        }

        // ────────────────────────────────────────────────────────────────────────
        // BULK OPERATIONS
        // ────────────────────────────────────────────────────────────────────────

        public async Task AddRangeAsync(IEnumerable<Specialization> specializations)
        {
            await _dbContext.Specializations.AddRangeAsync(specializations);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(IEnumerable<Specialization> specializations)
        {
            _dbContext.Specializations.UpdateRange(specializations);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<Specialization> specializations)
        {
            _dbContext.Specializations.RemoveRange(specializations);
            await _dbContext.SaveChangesAsync();
        }

        // ────────────────────────────────────────────────────────────────────────
        // PRIVATE HELPERS
        // ────────────────────────────────────────────────────────────────────────

        private IQueryable<Specialization> ApplyDepartmentFilters(
            IQueryable<Specialization> query,
            SpecializationDepartmentQueries? filter)
        {
            if (filter == null) return query;

            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(s => s.Name.Contains(filter.Name));

            if (filter.HasStudents.HasValue)
            {
                if (filter.HasStudents.Value)
                    query = query.Where(s => s.Students.Any());
                else
                    query = query.Where(s => !s.Students.Any());
            }

            if (filter.HasCourses.HasValue)
            {
                if (filter.HasCourses.Value)
                    query = query.Where(s => s.SpecializationCourses.Any());
                else
                    query = query.Where(s => !s.SpecializationCourses.Any());
            }

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                var searchTerm = filter.SearchTerm.ToLower();
                query = query.Where(s =>
                    s.Name.ToLower().Contains(searchTerm) ||
                    s.Description.ToLower().Contains(searchTerm) ||
                    s.Department.Name.ToLower().Contains(searchTerm));
            }

            return query;
        }

        private IQueryable<Specialization> ApplyGlobalFilters(
            IQueryable<Specialization> query,
            SpecializationFilterQueries? filter)
        {
            if (filter == null) return query;

            if (filter.DepartmentId.HasValue)
                query = query.Where(s => s.DepartmentId == filter.DepartmentId.Value);

            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(s => s.Name.Contains(filter.Name));

            if (filter.HasStudents.HasValue)
            {
                if (filter.HasStudents.Value)
                    query = query.Where(s => s.Students.Any());
                else
                    query = query.Where(s => !s.Students.Any());
            }

            if (filter.HasCourses.HasValue)
            {
                if (filter.HasCourses.Value)
                    query = query.Where(s => s.SpecializationCourses.Any());
                else
                    query = query.Where(s => !s.SpecializationCourses.Any());
            }

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                var searchTerm = filter.SearchTerm.ToLower();
                query = query.Where(s =>
                    s.Name.ToLower().Contains(searchTerm) ||
                    s.Description.ToLower().Contains(searchTerm) ||
                    s.Department.Name.ToLower().Contains(searchTerm));
            }

            return query;
        }
    }
}