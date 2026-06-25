using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Enums;
using University_Management_System.Domain.Queries.CourseQueries;

namespace University_Management_System.Domain.Contracts
{
    public interface ICourseRepository : IGenericRepository<Course, int>
    {
        // ─── Get by criteria ──────────────────────────────────────────────────
        Task<Course?> GetCourseWithDetailsAsync(int courseId);
        Task<Course?> GetCourseWithUploadsAsync(int courseId);
        Task<Course?> GetCourseWithPrerequisitesAsync(int courseId);
        
        // ─── Get collections with filters ────────────────────────────────────
        Task<(IEnumerable<Course> Data, int TotalCount)> GetFilteredAsync(
            CourseFilterQueries query,
            CancellationToken cancellationToken = default);
        
        // ─── Get by department ────────────────────────────────────────────────
        Task<(IEnumerable<Course> Data, int TotalCount)> GetByDepartmentAsync(
            int departmentId,
            CourseDepartmentQueries query,
            CancellationToken cancellationToken = default);
        
        // ─── Get course dependencies ──────────────────────────────────────────
        Task<IEnumerable<Course>> GetDependenciesAsync(int courseId);
        Task<IEnumerable<Course>> GetPrerequisitesAsync(int courseId);
        Task<IEnumerable<CoursePrerequisite>> GetPrerequisiteMappingsAsync();
        Task<IEnumerable<CoursePrerequisite>> GetPrerequisiteMappingsForOpenCoursesAsync();
        
        // ─── Get open courses ──────────────────────────────────────────────────
        Task<IEnumerable<Course>> GetOpenCoursesAsync();
        Task<IEnumerable<Course>> GetAllPrerequisitesForOpenCoursesAsync();
        
        // ─── Get passed courses ──────────────────────────────────────────────
        Task<IEnumerable<Course>> GetPassedCoursesByStudentIdAsync(string studentId);
        
        // ─── Update operations ────────────────────────────────────────────────
        Task UpdateStatusAsync(int courseId, CourseStatus newStatus);
        
        // ─── Check existence ──────────────────────────────────────────────────
        Task<bool> CourseExistsAsync(int courseId);
        Task<bool> CourseCodeExistsAsync(string code);
        Task<bool> HasPrerequisitesAsync(int courseId);
        Task<bool> HasDependenciesAsync(int courseId);
        
        // ─── Counts ────────────────────────────────────────────────────────────
        Task<int> GetCourseCountByDepartmentAsync(int departmentId);
        Task<int> GetCourseCountByStatusAsync(CourseStatus status);
        
        // ─── Bulk operations ──────────────────────────────────────────────────
        Task AddRangeAsync(IEnumerable<Course> courses);
        Task UpdateRangeAsync(IEnumerable<Course> courses);
        Task DeleteRangeAsync(IEnumerable<Course> courses);
    }
}