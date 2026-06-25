using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Domain.Queries.AdminQueries;

namespace University_Management_System.Domain.Contracts
{
    public interface IAdminRepository : IGenericRepository<Admin, string>
    {
        // ─── Get by criteria ──────────────────────────────────────────────────
        Task<Admin?> GetAdminByUserIdAsync(string userId);
        
        // ─── Get collections with filters and pagination ────────────────────
        Task<(IEnumerable<Admin> Data, int TotalCount)> GetAllFilteredAsync(
            AdminFilterQueries query,
            CancellationToken cancellationToken = default);
        
        // ─── Check existence ──────────────────────────────────────────────────
        Task<bool> AdminExistsAsync(string adminId);
        Task<bool> IsAdminActiveAsync(string adminId);
    }
}