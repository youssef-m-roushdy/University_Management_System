using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Enums;
using University_Management_System.Domain.Queries.DepartmentCourseQueries;

namespace University_Management_System.Domain.Contracts
{
    public interface IDepartmentCourseRepository : IGenericRepository<DepartmentCourse, int>
    {
        // ─── Get by department ──────────────────────────────────────────────────
        Task<IEnumerable<DepartmentCourse>> GetByDepartmentIdAsync(int departmentId);
        Task<IEnumerable<DepartmentCourse>> GetByDepartmentIdWithDetailsAsync(int departmentId);
        Task<(IEnumerable<DepartmentCourse> Data, int TotalCount)> GetCoursesByDepartmentIdAsync(
            int departmentId,
            DepartmentCourseFilterQueries? filter = null,
            CancellationToken cancellationToken = default);
        
        Task<(IEnumerable<DepartmentCourse> Data, int TotalCount)> GetAllDepartmentCoursesAsync(
            CourseFilterInDepartmentQueries? filter = null,
            CancellationToken cancellationToken = default);
        
        // ─── Get by course ──────────────────────────────────────────────────
        Task<IEnumerable<DepartmentCourse>> GetByCourseIdAsync(int courseId);
        Task<IEnumerable<DepartmentCourse>> GetByCourseIdWithDetailsAsync(int courseId);
        
        // ─── Get by role ──────────────────────────────────────────────────
        Task<IEnumerable<DepartmentCourse>> GetByRoleAsync(CourseRole role);
        Task<IEnumerable<DepartmentCourse>> GetByDepartmentAndRoleAsync(int departmentId, CourseRole role);
        
        // ─── Check existence ──────────────────────────────────────────────────
        Task<bool> ExistsAsync(int departmentId, int courseId);
        Task<bool> ExistsByDepartmentAndRoleAsync(int departmentId, CourseRole role);
        
        // ─── Delete operations ──────────────────────────────────────────────────
        Task DeleteByDepartmentIdAsync(int departmentId);
        Task DeleteByCourseIdAsync(int courseId);
        Task DeleteRangeAsync(IEnumerable<DepartmentCourse> departmentCourses);
    }
}