using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Domain.Queries.InstructorQueries;

namespace University_Management_System.Domain.Contracts
{
    public interface IInstructorRepository : IGenericRepository<Instructor, string>
    {
        // ─── Get by criteria ──────────────────────────────────────────────────
        Task<Instructor?> GetInstructorByUserIdAsync(string userId);
        Task<Instructor?> GetInstructorByDepartmentIdAsync(int departmentId);
        
        // ─── Get collections with filters and pagination ────────────────────
        Task<(IEnumerable<Instructor> Data, int TotalCount)> GetAllFilteredAsync(
            InstructorFilterQueries query,
            CancellationToken cancellationToken = default);
        
        // ✅ NEW: Get instructors in a specific department
        Task<(IEnumerable<Instructor> Data, int TotalCount)> GetDepartmentInstructorsAsync(
            int departmentId,
            InstructorDepartmentQueries query,
            CancellationToken cancellationToken = default);
        
        // ─── Get counts ──────────────────────────────────────────────────────
        Task<int> GetInstructorCountByDepartmentAsync(int departmentId);
        
        // ─── Check existence ──────────────────────────────────────────────────
        Task<bool> InstructorExistsAsync(string instructorId);
        Task<bool> IsInstructorActiveAsync(string instructorId);
        
        // ─── Partial Updates ──────────────────────────────────────────────────
        Task<Instructor> UpdateInstructorDepartmentAsync(string instructorId, int departmentId);
    }
}