using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Domain.Queries.AdminQueries;
using Microsoft.EntityFrameworkCore;
using University_Management_System.Domain.Queries;

namespace University_Management_System.Infrastructure.Presistence.Repositories
{
    public class AdminRepository : GenericRepository<Admin, string>, IAdminRepository
    {
        public AdminRepository(UniversityDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Admin?> GetAdminByUserIdAsync(string userId)
        {
            return await _dbContext.Admins
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == userId);
        }

        public async Task<(IEnumerable<Admin> Data, int TotalCount)> GetAllFilteredAsync(
            AdminFilterQueries query,
            CancellationToken cancellationToken = default)
        {
            var queryable = _dbContext.Admins
                .Include(a => a.User)
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
                "email" => query.SortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(a => a.User.Email)
                    : queryable.OrderByDescending(a => a.User.Email),
                _ => queryable.OrderBy(a => a.User.Name)
            };

            var result = await queryable
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);

            return (result, totalCount);
        }

        public async Task<bool> AdminExistsAsync(string adminId)
        {
            return await _dbContext.Admins.AnyAsync(a => a.Id == adminId);
        }

        public async Task<bool> IsAdminActiveAsync(string adminId)
        {
            var admin = await GetAdminByUserIdAsync(adminId);
            return admin?.User.IsActive ?? false;
        }
    }
}