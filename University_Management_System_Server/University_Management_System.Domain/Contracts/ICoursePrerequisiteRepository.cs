using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Domain.Contracts
{
    public interface ICoursePrerequisiteRepository : IGenericRepository<CoursePrerequisite, int>
    {
        // ─── Get by course ──────────────────────────────────────────────────
        Task<IEnumerable<CoursePrerequisite>> GetByCourseIdAsync(int courseId);
        Task<IEnumerable<CoursePrerequisite>> GetByPrerequisiteCourseIdAsync(int prerequisiteCourseId);
        
        // ─── Get with details ──────────────────────────────────────────────
        Task<IEnumerable<CoursePrerequisite>> GetByCourseIdWithDetailsAsync(int courseId);
        Task<IEnumerable<CoursePrerequisite>> GetByPrerequisiteCourseIdWithDetailsAsync(int prerequisiteCourseId);
        
        // ─── Check existence ──────────────────────────────────────────────
        Task<bool> ExistsAsync(int courseId, int prerequisiteCourseId);
        Task<bool> HasPrerequisitesAsync(int courseId);
        Task<bool> HasDependenciesAsync(int courseId);
        
        // ─── Delete operations ─────────────────────────────────────────────
        Task DeleteByCourseIdAsync(int courseId);
        Task DeleteRangeAsync(IEnumerable<CoursePrerequisite> prerequisites);
    }
}