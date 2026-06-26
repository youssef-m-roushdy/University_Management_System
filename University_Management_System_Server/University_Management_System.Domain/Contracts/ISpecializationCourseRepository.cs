using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Enums;
using University_Management_System.Domain.Queries.SpecializationCourseQueries;

namespace University_Management_System.Domain.Contracts
{
    public interface ISpecializationCourseRepository : IGenericRepository<SpecializationCourse, int>
    {
        // ─── Get by specialization ──────────────────────────────────────────────
        Task<IEnumerable<SpecializationCourse>> GetBySpecializationIdAsync(int specializationId);
        Task<IEnumerable<SpecializationCourse>> GetBySpecializationIdWithDetailsAsync(int specializationId);
        Task<(IEnumerable<SpecializationCourse> Data, int TotalCount)> GetBySpecializationIdAsync(
            int specializationId,
            CourseFilterInSpecailizationQueries? filter = null,
            CancellationToken cancellationToken = default);
        
        // ─── Get by course ──────────────────────────────────────────────────
        Task<IEnumerable<SpecializationCourse>> GetByCourseIdAsync(int courseId);
        Task<IEnumerable<SpecializationCourse>> GetByCourseIdWithDetailsAsync(int courseId);
        
        // ─── Get by role ──────────────────────────────────────────────────
        Task<IEnumerable<SpecializationCourse>> GetByRoleAsync(SpecializationCourseRole role);
        Task<IEnumerable<SpecializationCourse>> GetBySpecializationAndRoleAsync(
            int specializationId, 
            SpecializationCourseRole role);
        
        // ─── Get all with filters ──────────────────────────────────────────────
        Task<(IEnumerable<SpecializationCourse> Data, int TotalCount)> GetAllFilteredAsync(
            SpecializationCourseFilterQueries? filter = null,
            CancellationToken cancellationToken = default);
        
        // ─── Check existence ──────────────────────────────────────────────────
        Task<bool> ExistsAsync(int specializationId, int courseId);
        Task<bool> ExistsBySpecializationAndRoleAsync(int specializationId, SpecializationCourseRole role);
        Task<bool> ExistsByIdAsync(int id);
        
        // ─── Counts ────────────────────────────────────────────────────────────
        Task<int> GetCountBySpecializationAsync(int specializationId);
        Task<int> GetCountByCourseAsync(int courseId);
        Task<int> GetCountByRoleAsync(SpecializationCourseRole role);
        
        // ─── Bulk operations ──────────────────────────────────────────────────
        Task AddRangeAsync(IEnumerable<SpecializationCourse> specializationCourses);
        Task UpdateRangeAsync(IEnumerable<SpecializationCourse> specializationCourses);
        Task DeleteRangeAsync(IEnumerable<SpecializationCourse> specializationCourses);
    }
}