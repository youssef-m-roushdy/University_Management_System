using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Enums;
using University_Management_System.Domain.Queries;

namespace University_Management_System.Domain.Contracts
{
    public interface IStudentStudyYearRepository : IGenericRepository<StudentStudyYear, int>
    {
        // ─── Get by student ────────────────────────────────────────────────────
        Task<IEnumerable<StudentStudyYear>> GetByStudentIdAsync(string studentId);
        Task<IEnumerable<StudentStudyYear>> GetActiveByStudentIdAsync(string studentId);
        Task<StudentStudyYear?> GetCurrentByStudentIdAsync(string studentId);
        Task<IEnumerable<StudentStudyYear>> GetStudyYearsByStudentIdAsync(string studentId);
        
        // ─── Get by study year ────────────────────────────────────────────────
        Task<IEnumerable<StudentStudyYear>> GetByStudyYearIdAsync(int studyYearId);
        Task<(IEnumerable<StudentStudyYear> Data, int TotalCount)> GetStudyYearStudentsByStudyYearIdAsync(
            int studyYearId,
            StudyYearStudentQueries query,
            CancellationToken cancellationToken = default);
        
        // ─── Get by both ──────────────────────────────────────────────────────
        Task<StudentStudyYear?> GetByStudentAndStudyYearAsync(string studentId, int studyYearId);
        
        // ─── Bulk operations ──────────────────────────────────────────────────
        Task AddRangeAsync(IEnumerable<StudentStudyYear> studentStudyYears);
        
        // ─── Check existence ──────────────────────────────────────────────────
        Task<bool> IsStudentEnrolledAsync(string studentId, int studyYearId);
        Task<bool> IsStudentActiveAsync(string studentId, int studyYearId);
        
        // ─── Counts ────────────────────────────────────────────────────────────
        Task<int> GetStudentCountByStudyYearAsync(int studyYearId);
        Task<int> GetStudentCountByLevelAsync(int studyYearId, Levels level, CancellationToken cancellationToken = default);
    }
}