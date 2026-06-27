using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Queries.AcademicScheduleQueries;

namespace University_Management_System.Domain.Contracts
{
    public interface IAcademicScheduleRepository : IGenericRepository<AcademicSchedule, int>
    {
        // ─── Get by department and semester (Student, Instructor, Assistant) ──
        Task<(IEnumerable<AcademicSchedule> Data, int TotalCount)> GetByDepartmentAndSemesterAsync(
            int departmentId,
            int semesterId,
            AcademicScheduleDepartmentSemesterQueries? filter = null,
            CancellationToken cancellationToken = default);

        // ✅ Add this method
        Task<IEnumerable<AcademicSchedule>> GetByDepartmentAndSemesterWithDetailsAsync(
            int departmentId,
            int semesterId);

        // ─── Get by department (Admin only) ──────────────────────────────────
        Task<(IEnumerable<AcademicSchedule> Data, int TotalCount)> GetByDepartmentIdAsync(
            int departmentId,
            AcademicScheduleSemesterQueries? filter = null,
            CancellationToken cancellationToken = default);

        // ─── Get by semester ──────────────────────────────────────────────────
        Task<IEnumerable<AcademicSchedule>> GetBySemesterIdAsync(int semesterId);

        // ─── Get by study year ──────────────────────────────────────────────
        Task<IEnumerable<AcademicSchedule>> GetByStudyYearIdAsync(int studyYearId);

        // ─── Check existence ──────────────────────────────────────────────────
        Task<bool> ExistsAsync(int departmentId, int semesterId, string title);
        Task<bool> ExistsByIdAsync(int id);
        // ─── Get by ID with details ──────────────────────────────────────────────
        Task<AcademicSchedule?> GetByIdWithDetailsAsync(int id);
    }
}