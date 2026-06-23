using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Enums;
using University_Management_System.Domain.Queries.FeeQueries;

namespace University_Management_System.Domain.Contracts
{
    public interface IFeeRepository : IGenericRepository<Fee, int>
    {
        // ─── Get by study year ──────────────────────────────────────────────────
        Task<(IEnumerable<Fee> Data, int TotalCount)> GetByStudyYearIdAsync(
            int studyYearId,
            FeeStudyYearQueries? filter = null,
            CancellationToken cancellationToken = default);
        
        // ─── Get by department ──────────────────────────────────────────────────
        Task<(IEnumerable<Fee> Data, int TotalCount)> GetByDepartmentIdAsync(
            int departmentId,
            FeeDepartmentQueries? filter = null,
            CancellationToken cancellationToken = default);
        
        // ─── Get by department and study year ──────────────────────────────────
        Task<(IEnumerable<Fee> Data, int TotalCount)> GetByDepartmentAndStudyYearAsync(
            int departmentId,
            int studyYearId,
            FeeDepartmentStudyYearQueries? filter = null,
            CancellationToken cancellationToken = default);
        
        // ─── Get by level ──────────────────────────────────────────────────────
        Task<(IEnumerable<Fee> Data, int TotalCount)> GetByLevelAsync(
            int level,
            FeeLevelQueries? filter = null,
            CancellationToken cancellationToken = default);
        
        // ─── Get by level and study year ──────────────────────────────────────
        Task<(IEnumerable<Fee> Data, int TotalCount)> GetByLevelAndStudyYearAsync(
            int level,
            int studyYearId,
            FeeLevelStudyYearQueries? filter = null,
            CancellationToken cancellationToken = default);
        
        // ─── Get all with pagination ──────────────────────────────────────────
        // By default the get all is same as git all paginated
        Task<(IEnumerable<Fee> Data, int TotalCount)> GetAllAsync(
            FeeFilterQueries? filter = null,
            CancellationToken cancellationToken = default);
        
        // ─── Check existence ──────────────────────────────────────────────────
        Task<bool> FeeExistsAsync(int studyYearId, int? departmentId, int level);
        Task<bool> FeeExistsByIdAsync(int feeId);
        
        // ─── Counts ────────────────────────────────────────────────────────────
        Task<decimal> GetTotalFeesByStudyYearAsync(int studyYearId);
        Task<decimal> GetTotalFeesByDepartmentAsync(int departmentId);
        Task<decimal> GetTotalFeesByLevelAsync(int level);
        
        // ─── Statistics ──────────────────────────────────────────────────────
        Task<Dictionary<Levels, decimal>> GetFeesByLevelAsync(int studyYearId);
        Task<Dictionary<string, decimal>> GetFeesByDepartmentAsync(int studyYearId);
        Task<decimal> GetAverageFeeByStudyYearAsync(int studyYearId);
        
        // ─── Bulk operations ──────────────────────────────────────────────────
        Task AddRangeAsync(IEnumerable<Fee> fees);
        Task UpdateRangeAsync(IEnumerable<Fee> fees);
        Task DeleteRangeAsync(IEnumerable<Fee> fees);
    }
}