using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Domain.Queries.AssistantQueries;

namespace University_Management_System.Domain.Contracts
{
    public interface IAssistantRepository : IGenericRepository<Assistant, string>
    {
        // ─── Get by criteria ──────────────────────────────────────────────────
        Task<Assistant?> GetAssistantByUserIdAsync(string userId);
        Task<Assistant?> GetAssistantByDepartmentIdAsync(int departmentId);
        
        // ─── Get collections with filters and pagination ────────────────────
        Task<(IEnumerable<Assistant> Data, int TotalCount)> GetAllFilteredAsync(
            AssistantFilterQueries query,
            CancellationToken cancellationToken = default);
        
        // ✅ NEW: Get assistants in a specific department
        Task<(IEnumerable<Assistant> Data, int TotalCount)> GetDepartmentAssistantsAsync(
            int departmentId,
            AssistantDepartmentQueries query,
            CancellationToken cancellationToken = default);
        
        // ─── Get counts ──────────────────────────────────────────────────────
        Task<int> GetAssistantCountByDepartmentAsync(int departmentId);
        
        // ─── Check existence ──────────────────────────────────────────────────
        Task<bool> AssistantExistsAsync(string assistantId);
        Task<bool> IsAssistantActiveAsync(string assistantId);
        
        // ─── Partial Updates ──────────────────────────────────────────────────
        Task<Assistant> UpdateAssistantDepartmentAsync(string assistantId, int departmentId);
    }
}