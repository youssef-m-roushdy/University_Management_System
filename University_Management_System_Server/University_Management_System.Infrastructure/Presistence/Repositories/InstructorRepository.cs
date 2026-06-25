using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Domain.Queries.InstructorQueries;
using Microsoft.EntityFrameworkCore;
using University_Management_System.Domain.Queries;

namespace University_Management_System.Infrastructure.Presistence.Repositories
{
    public class InstructorRepository : GenericRepository<Instructor, string>, IInstructorRepository
    {
        public InstructorRepository(UniversityDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Instructor?> GetInstructorByUserIdAsync(string userId)
        {
            return await _dbContext.Instructors
                .Include(i => i.User)
                .Include(i => i.Department)
                .FirstOrDefaultAsync(i => i.Id == userId);
        }

        public async Task<Instructor?> GetInstructorByDepartmentIdAsync(int departmentId)
        {
            return await _dbContext.Instructors
                .Include(i => i.User)
                .FirstOrDefaultAsync(i => i.DepartmentId == departmentId);
        }

        public async Task<(IEnumerable<Instructor> Data, int TotalCount)> GetAllFilteredAsync(
            InstructorFilterQueries query,
            CancellationToken cancellationToken = default)
        {
            var queryable = _dbContext.Instructors
                .Include(i => i.User)
                .Include(i => i.Department)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(query.Name))
            {
                var searchTerm = query.Name.ToLower();
                queryable = queryable.Where(i => i.User.Name.ToLower().Contains(searchTerm));
            }

            if (query.Gender.HasValue)
                queryable = queryable.Where(i => i.User.Gender == query.Gender.Value);

            if (query.DepartmentId.HasValue)
                queryable = queryable.Where(i => i.DepartmentId == query.DepartmentId.Value);

            if (!string.IsNullOrEmpty(query.DepartmentSearch))
            {
                var searchTerm = query.DepartmentSearch.ToLower();
                queryable = queryable.Where(i =>
                    i.Department.Name.ToLower().Contains(searchTerm) ||
                    i.Department.Code.ToLower().Contains(searchTerm));
            }

            if (query.IsActive.HasValue)
                queryable = queryable.Where(i => i.User.IsActive == query.IsActive.Value);

            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                var searchTerm = query.SearchTerm.ToLower();
                queryable = queryable.Where(i =>
                    i.User.Name.ToLower().Contains(searchTerm) ||
                    i.User.Email.ToLower().Contains(searchTerm) ||
                    i.Department.Name.ToLower().Contains(searchTerm));
            }

            var totalCount = await queryable.CountAsync(cancellationToken);

            queryable = query.SortBy?.ToLower() switch
            {
                "name" => query.SortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(i => i.User.Name)
                    : queryable.OrderByDescending(i => i.User.Name),
                "department" => query.SortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(i => i.Department.Name)
                    : queryable.OrderByDescending(i => i.Department.Name),
                _ => queryable.OrderBy(i => i.User.Name)
            };

            var result = await queryable
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);

            return (result, totalCount);
        }

        public async Task<(IEnumerable<Instructor> Data, int TotalCount)> GetDepartmentInstructorsAsync(
            int departmentId,
            InstructorDepartmentQueries query,
            CancellationToken cancellationToken = default)
        {
            var queryable = _dbContext.Instructors
                .Include(i => i.User)
                .Where(i => i.DepartmentId == departmentId)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(query.Name))
            {
                var searchTerm = query.Name.ToLower();
                queryable = queryable.Where(i => i.User.Name.ToLower().Contains(searchTerm));
            }

            if (query.Gender.HasValue)
                queryable = queryable.Where(i => i.User.Gender == query.Gender.Value);

            if (query.IsActive.HasValue)
                queryable = queryable.Where(i => i.User.IsActive == query.IsActive.Value);

            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                var searchTerm = query.SearchTerm.ToLower();
                queryable = queryable.Where(i =>
                    i.User.Name.ToLower().Contains(searchTerm) ||
                    i.User.Email.ToLower().Contains(searchTerm));
            }

            var totalCount = await queryable.CountAsync(cancellationToken);

            queryable = query.SortBy?.ToLower() switch
            {
                "name" => query.SortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(i => i.User.Name)
                    : queryable.OrderByDescending(i => i.User.Name),
                _ => queryable.OrderBy(i => i.User.Name)
            };

            var result = await queryable
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);

            return (result, totalCount);
        }

        public async Task<int> GetInstructorCountByDepartmentAsync(int departmentId)
        {
            return await _dbContext.Instructors
                .CountAsync(i => i.DepartmentId == departmentId);
        }

        public async Task<bool> InstructorExistsAsync(string instructorId)
        {
            return await _dbContext.Instructors.AnyAsync(i => i.Id == instructorId);
        }

        public async Task<bool> IsInstructorActiveAsync(string instructorId)
        {
            var instructor = await GetInstructorByUserIdAsync(instructorId);
            return instructor?.User.IsActive ?? false;
        }

        public async Task<Instructor> UpdateInstructorDepartmentAsync(string instructorId, int departmentId)
        {
            var instructor = await _dbContext.Instructors
                .FirstOrDefaultAsync(i => i.Id == instructorId);

            if (instructor != null)
            {
                instructor.DepartmentId = departmentId;
                await _dbContext.SaveChangesAsync();
            }

            return instructor!;
        }
    }
}