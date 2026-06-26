using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Queries.SpecializationQueries;

namespace University_Management_System.Domain.Contracts
{
    public interface ISpecializationRepository : IGenericRepository<Specialization, int>
    {
        // ─── Get by department ──────────────────────────────────────────────────
        Task<IEnumerable<Specialization>> GetByDepartmentIdAsync(int departmentId);
        Task<IEnumerable<Specialization>> GetByDepartmentIdWithDetailsAsync(int departmentId);
        Task<(IEnumerable<Specialization> Data, int TotalCount)> GetByDepartmentIdAsync(
            int departmentId,
            SpecializationDepartmentQueries? filter = null,
            CancellationToken cancellationToken = default);
        
        // ─── Get all with filters ──────────────────────────────────────────────
        Task<(IEnumerable<Specialization> Data, int TotalCount)> GetAllFilteredAsync(
            SpecializationFilterQueries? filter = null,
            CancellationToken cancellationToken = default);
        
        // ─── Get by name ──────────────────────────────────────────────────────
        Task<Specialization?> GetByNameAsync(string name);
        Task<Specialization?> GetByNameWithDetailsAsync(string name);
        
        // ─── Check existence ──────────────────────────────────────────────────
        Task<bool> ExistsAsync(string name);
        Task<bool> ExistsByNameAndDepartmentAsync(string name, int departmentId);
        Task<bool> ExistsByIdAsync(int specializationId);
        
        // ─── Counts ────────────────────────────────────────────────────────────
        Task<int> GetCountByDepartmentAsync(int departmentId);
        Task<int> GetStudentCountAsync(int specializationId);
        Task<int> GetCourseCountAsync(int specializationId);
        
        // ─── Bulk operations ──────────────────────────────────────────────────
        Task AddRangeAsync(IEnumerable<Specialization> specializations);
        Task UpdateRangeAsync(IEnumerable<Specialization> specializations);
        Task DeleteRangeAsync(IEnumerable<Specialization> specializations);
    }
}