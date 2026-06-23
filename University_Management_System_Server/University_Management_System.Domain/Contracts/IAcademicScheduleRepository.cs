using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Queries;
using University_Management_System.Domain.Queries.AcademicScheduleQueries;

namespace University_Management_System.Domain.Contracts
{
    public interface IAcademicScheduleRepository : IGenericRepository<AcademicSchedule, int>
    {
        // ─── Get by study year ──────────────────────────────────────────────────
        Task<(IEnumerable<AcademicSchedule> Data, int TotalCount)> GetByStudyYearIdAsync(
            int studyYearId,
            AcademicScheduleStudyYearQueries? filter = null,
            CancellationToken cancellationToken = default);
        
        // ─── Get by semester ────────────────────────────────────────────────────
        Task<(IEnumerable<AcademicSchedule> Data, int TotalCount)> GetBySemesterIdAsync(
            int semesterId,
            AcademicScheduleSemesterQueries? filter = null,
            CancellationToken cancellationToken = default);
        
        // ─── Get by department ──────────────────────────────────────────────────
        Task<(IEnumerable<AcademicSchedule> Data, int TotalCount)> GetByDepartmentIdAsync(
            int departmentId,
            AcademicScheduleDepartmentQueries? filter = null,
            CancellationToken cancellationToken = default);
        
        // ─── Get by department and semester ────────────────────────────────────
        Task<(IEnumerable<AcademicSchedule> Data, int TotalCount)> GetByDepartmentAndSemesterAsync(
            int departmentId,
            int semesterId,
            AcademicScheduleDepartmentSemesterQueries? filter = null,
            CancellationToken cancellationToken = default);
        
        // ─── Get by date range ──────────────────────────────────────────────────
       
        
        // ─── Get all with pagination ──────────────────────────────────────────
        Task<(IEnumerable<AcademicSchedule> Data, int TotalCount)> GetAllAsync(
            AcademicScheduleFilterQueries? filter = null,
            CancellationToken cancellationToken = default);
        
        // ─── Check existence ──────────────────────────────────────────────────
        Task<bool> ScheduleExistsAsync(int studyYearId, int semesterId, int departmentId);
        Task<bool> ScheduleExistsByIdAsync(int scheduleId);
        
        // ─── Counts ────────────────────────────────────────────────────────────
        Task<int> GetCountByStudyYearAsync(int studyYearId);
        Task<int> GetCountBySemesterAsync(int semesterId);
        Task<int> GetCountByDepartmentAsync(int departmentId);
        
        // ─── Bulk operations ──────────────────────────────────────────────────
        Task AddRangeAsync(IEnumerable<AcademicSchedule> schedules);
        Task UpdateRangeAsync(IEnumerable<AcademicSchedule> schedules);
        Task DeleteRangeAsync(IEnumerable<AcademicSchedule> schedules);
    }
}