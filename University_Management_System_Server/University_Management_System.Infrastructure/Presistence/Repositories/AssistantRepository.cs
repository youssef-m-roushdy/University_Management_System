using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Domain.Queries.AssistantQueries;
using Microsoft.EntityFrameworkCore;
using University_Management_System.Domain.Queries;

namespace University_Management_System.Infrastructure.Presistence.Repositories
{
    public class AssistantRepository : GenericRepository<Assistant, string>, IAssistantRepository
    {
        public AssistantRepository(UniversityDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Assistant?> GetAssistantByUserIdAsync(string userId)
        {
            return await _dbContext.Assistants
                .Include(a => a.User)
                .Include(a => a.Department)
                .FirstOrDefaultAsync(a => a.Id == userId);
        }

        public async Task<Assistant?> GetAssistantByDepartmentIdAsync(int departmentId)
        {
            return await _dbContext.Assistants
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.DepartmentId == departmentId);
        }

        public async Task<(IEnumerable<Assistant> Data, int TotalCount)> GetAllFilteredAsync(
            AssistantFilterQueries query,
            CancellationToken cancellationToken = default)
        {
            var queryable = _dbContext.Assistants
                .Include(a => a.User)
                .Include(a => a.Department)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(query.Name))
            {
                var searchTerm = query.Name.ToLower();
                queryable = queryable.Where(a => a.User.Name.ToLower().Contains(searchTerm));
            }

            if (query.Gender.HasValue)
                queryable = queryable.Where(a => a.User.Gender == query.Gender.Value);

            if (query.DepartmentId.HasValue)
                queryable = queryable.Where(a => a.DepartmentId == query.DepartmentId.Value);

            if (!string.IsNullOrEmpty(query.DepartmentSearch))
            {
                var searchTerm = query.DepartmentSearch.ToLower();
                queryable = queryable.Where(a =>
                    a.Department.Name.ToLower().Contains(searchTerm) ||
                    a.Department.Code.ToLower().Contains(searchTerm));
            }

            if (query.IsActive.HasValue)
                queryable = queryable.Where(a => a.User.IsActive == query.IsActive.Value);

            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                var searchTerm = query.SearchTerm.ToLower();
                queryable = queryable.Where(a =>
                    a.User.Name.ToLower().Contains(searchTerm) ||
                    a.User.Email.ToLower().Contains(searchTerm) ||
                    a.Department.Name.ToLower().Contains(searchTerm));
            }

            var totalCount = await queryable.CountAsync(cancellationToken);

            queryable = query.SortBy?.ToLower() switch
            {
                "name" => query.SortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(a => a.User.Name)
                    : queryable.OrderByDescending(a => a.User.Name),
                "department" => query.SortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(a => a.Department.Name)
                    : queryable.OrderByDescending(a => a.Department.Name),
                _ => queryable.OrderBy(a => a.User.Name)
            };

            var result = await queryable
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);

            return (result, totalCount);
        }

        public async Task<(IEnumerable<Assistant> Data, int TotalCount)> GetDepartmentAssistantsAsync(
            int departmentId,
            AssistantDepartmentQueries query,
            CancellationToken cancellationToken = default)
        {
            var queryable = _dbContext.Assistants
                .Include(a => a.User)
                .Where(a => a.DepartmentId == departmentId)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(query.Name))
            {
                var searchTerm = query.Name.ToLower();
                queryable = queryable.Where(a => a.User.Name.ToLower().Contains(searchTerm));
            }

            if (query.Gender.HasValue)
                queryable = queryable.Where(a => a.User.Gender == query.Gender.Value);

            if (query.IsActive.HasValue)
                queryable = queryable.Where(a => a.User.IsActive == query.IsActive.Value);

            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                var searchTerm = query.SearchTerm.ToLower();
                queryable = queryable.Where(a =>
                    a.User.Name.ToLower().Contains(searchTerm) ||
                    a.User.Email.ToLower().Contains(searchTerm));
            }

            var totalCount = await queryable.CountAsync(cancellationToken);

            queryable = query.SortBy?.ToLower() switch
            {
                "name" => query.SortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(a => a.User.Name)
                    : queryable.OrderByDescending(a => a.User.Name),
                _ => queryable.OrderBy(a => a.User.Name)
            };

            var result = await queryable
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);

            return (result, totalCount);
        }

        public async Task<int> GetAssistantCountByDepartmentAsync(int departmentId)
        {
            return await _dbContext.Assistants
                .CountAsync(a => a.DepartmentId == departmentId);
        }

        public async Task<bool> AssistantExistsAsync(string assistantId)
        {
            return await _dbContext.Assistants.AnyAsync(a => a.Id == assistantId);
        }

        public async Task<bool> IsAssistantActiveAsync(string assistantId)
        {
            var assistant = await GetAssistantByUserIdAsync(assistantId);
            return assistant?.User.IsActive ?? false;
        }

        public async Task<Assistant> UpdateAssistantDepartmentAsync(string assistantId, int departmentId)
        {
            var assistant = await _dbContext.Assistants
                .FirstOrDefaultAsync(a => a.Id == assistantId);

            if (assistant != null)
            {
                assistant.DepartmentId = departmentId;
                await _dbContext.SaveChangesAsync();
            }

            return assistant!;
        }
    }
}