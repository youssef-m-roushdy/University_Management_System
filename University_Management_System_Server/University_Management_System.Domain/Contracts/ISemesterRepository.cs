using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Enums;
using University_Management_System.Domain.Queries;
using University_Management_System.Domain.Queries.SemesterQueries;

namespace University_Management_System.Domain.Contracts
{
    public interface ISemesterRepository : IGenericRepository<Semester, int>
    {
        // ─── Check existence ──────────────────────────────────────────────────
        Task<bool> IsActiveSemesterAsync(int semesterId);
        Task<bool> IsSemesterBelongsToStudyYearAsync(int semesterId, int studyYearId);
        Task<bool> SemesterTitleExistsInStudyYearAsync(int studyYearId, SemesterEnum title);
        
        // ─── Get collections ──────────────────────────────────────────────────
        Task<IEnumerable<Semester>> GetByStudyYearIdAsync(int studyYearId);
        
        // ─── Get with pagination ──────────────────────────────────────────────
        Task<(IEnumerable<Semester> Data, int TotalCount)> GetAllFilteredAsync(
            SemesterFilterQueries query, 
            CancellationToken cancellationToken = default);
        
        // ─── Create ────────────────────────────────────────────────────────────
        Task<Semester> CreateStudyYearSemesterAsync(int studyYearId, Semester semester);
        
        // ─── Get with details ──────────────────────────────────────────────────
        Task<Semester?> GetSemesterWithDetailsAsync(int semesterId, CancellationToken cancellationToken = default);
        Task<Semester?> GetSemesterWithFullDetailsAsync(int semesterId, CancellationToken cancellationToken = default);
    }
}