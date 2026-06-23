using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;
using University_Management_System.Domain.Queries;
using University_Management_System.Domain.Queries.FeeQueries;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Infrastructure.Presistence.Repositories
{
    public class FeeRepository : GenericRepository<Fee, int>, IFeeRepository
    {
        public FeeRepository(UniversityDbContext dbContext) : base(dbContext)
        {
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY STUDY YEAR
        // ────────────────────────────────────────────────────────────────────────

        public async Task<(IEnumerable<Fee> Data, int TotalCount)> GetByStudyYearIdAsync(
            int studyYearId,
            FeeStudyYearQueries? filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = BuildBaseQuery()
                .Where(f => f.StudyYearId == studyYearId);

            query = ApplyStudyYearFilters(query, filter);

            return await ApplyPaginationAsync(query, filter, cancellationToken);
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY DEPARTMENT
        // ────────────────────────────────────────────────────────────────────────

        public async Task<(IEnumerable<Fee> Data, int TotalCount)> GetByDepartmentIdAsync(
            int departmentId,
            FeeDepartmentQueries? filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = BuildBaseQuery()
                .Where(f => f.DepartmentId == departmentId);

            query = ApplyDepartmentFilters(query, filter);

            return await ApplyPaginationAsync(query, filter, cancellationToken);
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY DEPARTMENT AND STUDY YEAR
        // ────────────────────────────────────────────────────────────────────────

        public async Task<(IEnumerable<Fee> Data, int TotalCount)> GetByDepartmentAndStudyYearAsync(
            int departmentId,
            int studyYearId,
            FeeDepartmentStudyYearQueries? filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = BuildBaseQuery()
                .Where(f => f.DepartmentId == departmentId && f.StudyYearId == studyYearId);

            query = ApplyDepartmentStudyYearFilters(query, filter);

            return await ApplyPaginationAsync(query, filter, cancellationToken);
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY LEVEL
        // ────────────────────────────────────────────────────────────────────────

        public async Task<(IEnumerable<Fee> Data, int TotalCount)> GetByLevelAsync(
            int level,
            FeeLevelQueries? filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = BuildBaseQuery()
                .Where(f => f.Level == (Levels)level);

            query = ApplyLevelFilters(query, filter);

            return await ApplyPaginationAsync(query, filter, cancellationToken);
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY LEVEL AND STUDY YEAR
        // ────────────────────────────────────────────────────────────────────────

        public async Task<(IEnumerable<Fee> Data, int TotalCount)> GetByLevelAndStudyYearAsync(
            int level,
            int studyYearId,
            FeeLevelStudyYearQueries? filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = BuildBaseQuery()
                .Where(f => f.Level == (Levels)level && f.StudyYearId == studyYearId);

            query = ApplyLevelStudyYearFilters(query, filter);

            return await ApplyPaginationAsync(query, filter, cancellationToken);
        }



        // ────────────────────────────────────────────────────────────────────────
        // GET ALL WITH PAGINATION
        // ────────────────────────────────────────────────────────────────────────

        public async Task<(IEnumerable<Fee> Data, int TotalCount)> GetAllAsync(
            FeeFilterQueries? filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = BuildBaseQuery();

            query = ApplyFilters(query, filter);

            return await ApplyPaginationAsync(query, filter, cancellationToken);
        }

        // ────────────────────────────────────────────────────────────────────────
        // CHECK EXISTENCE
        // ────────────────────────────────────────────────────────────────────────

        public async Task<bool> FeeExistsAsync(int studyYearId, int? departmentId, int level)
        {
            var query = _dbContext.Fees
                .Where(f => f.StudyYearId == studyYearId && f.Level == (Levels)level);

            if (departmentId.HasValue)
                query = query.Where(f => f.DepartmentId == departmentId.Value);

            return await query.AnyAsync();
        }

        public async Task<bool> FeeExistsByIdAsync(int feeId)
        {
            return await _dbContext.Fees.AnyAsync(f => f.Id == feeId);
        }

        // ────────────────────────────────────────────────────────────────────────
        // COUNTS
        // ────────────────────────────────────────────────────────────────────────

        public async Task<decimal> GetTotalFeesByStudyYearAsync(int studyYearId)
        {
            return await _dbContext.Fees
                .Where(f => f.StudyYearId == studyYearId)
                .SumAsync(f => f.Amount);
        }

        public async Task<decimal> GetTotalFeesByDepartmentAsync(int departmentId)
        {
            return await _dbContext.Fees
                .Where(f => f.DepartmentId == departmentId)
                .SumAsync(f => f.Amount);
        }

        public async Task<decimal> GetTotalFeesByLevelAsync(int level)
        {
            return await _dbContext.Fees
                .Where(f => f.Level == (Levels)level)
                .SumAsync(f => f.Amount);
        }

        // ────────────────────────────────────────────────────────────────────────
        // STATISTICS
        // ────────────────────────────────────────────────────────────────────────

        public async Task<Dictionary<Levels, decimal>> GetFeesByLevelAsync(int studyYearId)
        {
            return await _dbContext.Fees
                .Where(f => f.StudyYearId == studyYearId)
                .GroupBy(f => f.Level)
                .Select(g => new
                {
                    Level = g.Key,
                    TotalAmount = g.Sum(f => f.Amount)
                })
                .ToDictionaryAsync(k => k.Level, v => v.TotalAmount);
        }
        public async Task<Dictionary<string, decimal>> GetFeesByDepartmentAsync(int studyYearId)
        {
            return await _dbContext.Fees
                .Where(f => f.StudyYearId == studyYearId)
                .Include(f => f.Department)
                .GroupBy(f => f.Department.Name)
                .Select(g => new
                {
                    DepartmentName = g.Key,
                    TotalAmount = g.Sum(f => f.Amount)
                })
                .ToDictionaryAsync(k => k.DepartmentName, v => v.TotalAmount);
        }

        public async Task<decimal> GetAverageFeeByStudyYearAsync(int studyYearId)
        {
            var fees = await _dbContext.Fees
                .Where(f => f.StudyYearId == studyYearId)
                .ToListAsync();

            if (!fees.Any())
                return 0;

            return fees.Average(f => f.Amount);
        }

        // ────────────────────────────────────────────────────────────────────────
        // BULK OPERATIONS
        // ────────────────────────────────────────────────────────────────────────

        public async Task AddRangeAsync(IEnumerable<Fee> fees)
        {
            await _dbContext.Fees.AddRangeAsync(fees);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(IEnumerable<Fee> fees)
        {
            _dbContext.Fees.UpdateRange(fees);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<Fee> fees)
        {
            _dbContext.Fees.RemoveRange(fees);
            await _dbContext.SaveChangesAsync();
        }

        // ────────────────────────────────────────────────────────────────────────
        // PRIVATE HELPERS
        // ────────────────────────────────────────────────────────────────────────

        private IQueryable<Fee> BuildBaseQuery()
        {
            return _dbContext.Fees
                .Include(f => f.Department)
                .Include(f => f.StudyYear)
                .AsNoTracking()
                .AsQueryable();
        }

        private IQueryable<Fee> ApplyFilters(IQueryable<Fee> query, FeeFilterQueries? filter)
        {
            if (filter == null) return query;

            // ─── Department Filters ──────────────────────────────────────────
            if (!string.IsNullOrEmpty(filter.DepartmentName))
                query = query.Where(f => f.Department.Name.Contains(filter.DepartmentName));

            if (!string.IsNullOrEmpty(filter.DepartmentCode))
                query = query.Where(f => f.Department.Code.Contains(filter.DepartmentCode));

            // ─── Fee Info Filters ──────────────────────────────────────────
            if (filter.Level.HasValue)
                query = query.Where(f => f.Level == filter.Level.Value);

            if (filter.FeeType.HasValue)
                query = query.Where(f => f.Type == filter.FeeType.Value);

            if (filter.MinAmount.HasValue)
                query = query.Where(f => f.Amount >= filter.MinAmount.Value);

            if (filter.MaxAmount.HasValue)
                query = query.Where(f => f.Amount <= filter.MaxAmount.Value);

            // ─── Study Year Range Filters ──────────────────────────────────
            if (filter.StudyYearStart.HasValue)
                query = query.Where(f => f.StudyYear.StartYear >= filter.StudyYearStart.Value);

            if (filter.StudyYearEnd.HasValue)
                query = query.Where(f => f.StudyYear.EndYear <= filter.StudyYearEnd.Value);

            return query;
        }

        private IQueryable<Fee> ApplyStudyYearFilters(IQueryable<Fee> query, FeeStudyYearQueries? filter)
        {
            if (filter == null) return query;

            // ─── Department Filters ──────────────────────────────────────────
            if (!string.IsNullOrEmpty(filter.DepartmentName))
                query = query.Where(f => f.Department.Name.Contains(filter.DepartmentName));

            if (!string.IsNullOrEmpty(filter.DepartmentCode))
                query = query.Where(f => f.Department.Code.Contains(filter.DepartmentCode));

            // ─── Fee Info Filters ──────────────────────────────────────────
            if (filter.Level.HasValue)
                query = query.Where(f => f.Level == filter.Level.Value);

            if (filter.FeeType.HasValue)
                query = query.Where(f => f.Type == filter.FeeType.Value);

            if (filter.MinAmount.HasValue)
                query = query.Where(f => f.Amount >= filter.MinAmount.Value);

            if (filter.MaxAmount.HasValue)
                query = query.Where(f => f.Amount <= filter.MaxAmount.Value);

            return query;
        }
        private IQueryable<Fee> ApplyDepartmentFilters(IQueryable<Fee> query, FeeDepartmentQueries? filter)
        {
            if (filter == null) return query;

            // ─── Fee Info Filters ──────────────────────────────────────────
            if (filter.Level.HasValue)
                query = query.Where(f => f.Level == filter.Level.Value);

            if (filter.FeeType.HasValue)
                query = query.Where(f => f.Type == filter.FeeType.Value);

            if (filter.MinAmount.HasValue)
                query = query.Where(f => f.Amount >= filter.MinAmount.Value);

            if (filter.MaxAmount.HasValue)
                query = query.Where(f => f.Amount <= filter.MaxAmount.Value);

            // ─── Study Year Range Filters ──────────────────────────────────
            if (filter.StudyYearStart.HasValue)
                query = query.Where(f => f.StudyYear.StartYear >= filter.StudyYearStart.Value);

            if (filter.StudyYearEnd.HasValue)
                query = query.Where(f => f.StudyYear.EndYear <= filter.StudyYearEnd.Value);

            return query;
        }
        private IQueryable<Fee> ApplyDepartmentStudyYearFilters(IQueryable<Fee> query, FeeDepartmentStudyYearQueries? filter)
        {
            if (filter == null) return query;

            // ─── Fee Info Filters ──────────────────────────────────────────
            if (filter.Level.HasValue)
                query = query.Where(f => f.Level == filter.Level.Value);

            if (filter.FeeType.HasValue)
                query = query.Where(f => f.Type == filter.FeeType.Value);

            if (filter.MinAmount.HasValue)
                query = query.Where(f => f.Amount >= filter.MinAmount.Value);

            if (filter.MaxAmount.HasValue)
                query = query.Where(f => f.Amount <= filter.MaxAmount.Value);

            return query;
        }
        private IQueryable<Fee> ApplyLevelFilters(IQueryable<Fee> query, FeeLevelQueries? filter)
        {
            if (filter == null) return query;

            // ─── Department Filters ──────────────────────────────────────────
            if (!string.IsNullOrEmpty(filter.DepartmentName))
                query = query.Where(f => f.Department.Name.Contains(filter.DepartmentName));

            if (!string.IsNullOrEmpty(filter.DepartmentCode))
                query = query.Where(f => f.Department.Code.Contains(filter.DepartmentCode));


            if (filter.FeeType.HasValue)
                query = query.Where(f => f.Type == filter.FeeType.Value);

            if (filter.MinAmount.HasValue)
                query = query.Where(f => f.Amount >= filter.MinAmount.Value);

            if (filter.MaxAmount.HasValue)
                query = query.Where(f => f.Amount <= filter.MaxAmount.Value);

            // ─── Study Year Range Filters ──────────────────────────────────
            if (filter.StudyYearStart.HasValue)
                query = query.Where(f => f.StudyYear.StartYear >= filter.StudyYearStart.Value);

            if (filter.StudyYearEnd.HasValue)
                query = query.Where(f => f.StudyYear.EndYear <= filter.StudyYearEnd.Value);

            return query;
        }

        private IQueryable<Fee> ApplyLevelStudyYearFilters(IQueryable<Fee> query, FeeLevelStudyYearQueries? filter)
        {
            if (filter == null) return query;

            // ─── Department Filters ──────────────────────────────────────────
            if (!string.IsNullOrEmpty(filter.DepartmentName))
                query = query.Where(f => f.Department.Name.Contains(filter.DepartmentName));

            if (!string.IsNullOrEmpty(filter.DepartmentCode))
                query = query.Where(f => f.Department.Code.Contains(filter.DepartmentCode));


            if (filter.FeeType.HasValue)
                query = query.Where(f => f.Type == filter.FeeType.Value);

            if (filter.MinAmount.HasValue)
                query = query.Where(f => f.Amount >= filter.MinAmount.Value);

            if (filter.MaxAmount.HasValue)
                query = query.Where(f => f.Amount <= filter.MaxAmount.Value);

            return query;
        }

        private async Task<(IEnumerable<Fee> Data, int TotalCount)> ApplyPaginationAsync(
            IQueryable<Fee> query,
            PaginationQuery? filter,
            CancellationToken cancellationToken)
        {
            var totalCount = await query.CountAsync(cancellationToken);

            var pageNumber = filter?.PageNumber ?? 1;
            var pageSize = filter?.PageSize ?? 10;

            // Apply sorting
            query = filter?.SortBy?.ToLower() switch
            {
                "amount" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(f => f.Amount)
                    : query.OrderByDescending(f => f.Amount),
                "level" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(f => f.Level)
                    : query.OrderByDescending(f => f.Level),
                "department" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(f => f.Department.Name)
                    : query.OrderByDescending(f => f.Department.Name),
                "departmentcode" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(f => f.Department.Code)
                    : query.OrderByDescending(f => f.Department.Code),
                "feetype" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(f => f.Type)
                    : query.OrderByDescending(f => f.Type),
                "studyyear" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(f => f.StudyYear.StartYear)
                    : query.OrderByDescending(f => f.StudyYear.StartYear),
                _ => query.OrderBy(f => f.Level)
            };

            var result = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (result, totalCount);
        }
    }
}