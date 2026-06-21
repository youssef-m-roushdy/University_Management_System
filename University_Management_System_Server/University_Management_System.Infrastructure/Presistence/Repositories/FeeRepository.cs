using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;
using University_Management_System.Domain.Queries;

namespace University_Management_System.Infrastructure.Presistence.Repositories
{
    public class FeeRepository : GenericRepository<Fee, int>, IFeeRepository
    {
        public FeeRepository(UniversityDbContext dbContext) : base(dbContext)
        {
        }


        public async Task<IEnumerable<Fee>> GetFeesOfDepartmentForStudyYear(int departmentId, int studyYearId)
        {
            return await _dbContext.Fees
                .Where(f => f.DepartmentId == departmentId && f.StudyYearId == studyYearId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<(IEnumerable<Fee> Data, int TotalCount)> GetFeesByStudyYearIdAsync(
            int studyYearId,
            StudyYearFeeQueries query,
            CancellationToken cancellationToken)
        {
            var fees = GetQueryable()
                .Include(f => f.Department)
                .Where(f => f.StudyYearId == studyYearId);

            if (query.DepartmentId.HasValue)
                fees = fees.Where(f => f.DepartmentId == query.DepartmentId.Value);

            if (query.Level.HasValue)
                fees = fees.Where(f => f.Level == query.Level.Value);

            if (query.FeeType.HasValue)
                fees = fees.Where(f => f.Type == query.FeeType.Value);

            if (query.MinAmount.HasValue)
                fees = fees.Where(f => f.Amount >= query.MinAmount.Value);

            if (query.MaxAmount.HasValue)
                fees = fees.Where(f => f.Amount <= query.MaxAmount.Value);

            var totalCount = await fees.CountAsync(cancellationToken);

            fees = query.SortBy?.ToLower() switch
            {
                "amount" => query.SortDirection == SortDirection.Ascending
                    ? fees.OrderBy(f => f.Amount)
                    : fees.OrderByDescending(f => f.Amount),
                "level" => query.SortDirection == SortDirection.Ascending
                    ? fees.OrderBy(f => f.Level)
                    : fees.OrderByDescending(f => f.Level),
                "department" => query.SortDirection == SortDirection.Ascending
                    ? fees.OrderBy(f => f.Department.Name)
                    : fees.OrderByDescending(f => f.Department.Name),
                _ => fees.OrderBy(f => f.Level)
            };

            var result = await fees
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);

            return (result, totalCount);
        }
    }
}
