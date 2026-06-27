using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Queries.SemesterGPAQueries;

namespace University_Management_System.Domain.Contracts
{
    public interface ISemesterGPARepository : IGenericRepository<SemesterGPA, int>
    {
        // ─── Get by student ──────────────────────────────────────────────────
        Task<IEnumerable<SemesterGPA>> GetByStudentIdAsync(string studentId);
        Task<IEnumerable<SemesterGPA>> GetByStudentIdWithDetailsAsync(string studentId);
        Task<(IEnumerable<SemesterGPA> Data, int TotalCount)> GetByStudentIdPaginatedAsync(
            string studentId,
            semesterGPAStudyYearQueries? filter = null,
            CancellationToken cancellationToken = default);
        
        // ─── Get by semester ──────────────────────────────────────────────────
        Task<IEnumerable<SemesterGPA>> GetBySemesterIdAsync(int semesterId);
        Task<(IEnumerable<SemesterGPA> Data, int TotalCount)> GetBySemesterIdPaginatedAsync(
            int semesterId,
            SemesterGPAFilterInSemesterQueries? filter = null,
            CancellationToken cancellationToken = default);
        
        // ─── Get by study year ──────────────────────────────────────────────────
        Task<IEnumerable<SemesterGPA>> GetByStudyYearIdAsync(int studyYearId);
        Task<(IEnumerable<SemesterGPA> Data, int TotalCount)> GetByStudyYearIdPaginatedAsync(
            int studyYearId,
            semesterGPAStudyYearQueries? filter = null,
            CancellationToken cancellationToken = default);
        
        // ─── Get by student and semester ──────────────────────────────────────
        Task<SemesterGPA?> GetByStudentAndSemesterAsync(string studentId, int semesterId);
        Task<SemesterGPA?> GetByStudentAndSemesterWithDetailsAsync(string studentId, int semesterId);
        
        // ─── Get by student and study year ──────────────────────────────────────
        Task<IEnumerable<SemesterGPA>> GetByStudentAndStudyYearAsync(string studentId, int studyYearId);
        
        // ─── Get all with filters ──────────────────────────────────────────────
        Task<(IEnumerable<SemesterGPA> Data, int TotalCount)> GetAllFilteredAsync(
            SemesterGPAFilterQueries? filter = null,
            CancellationToken cancellationToken = default);
        
        // ─── Check existence ──────────────────────────────────────────────────
        Task<bool> ExistsAsync(string studentId, int semesterId);
        Task<bool> ExistsByIdAsync(int id);
        
        // ─── Get latest ──────────────────────────────────────────────────────
        Task<SemesterGPA?> GetLatestByStudentIdAsync(string studentId);
        
        // ─── Get cumulative GPA ──────────────────────────────────────────────
        Task<decimal> GetCumulativeGPAAsync(string studentId);
    }
}