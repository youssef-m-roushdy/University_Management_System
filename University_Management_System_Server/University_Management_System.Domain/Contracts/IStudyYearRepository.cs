using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Queries;
using University_Management_System.Domain.Queries.StudyYearQueries;

namespace University_Management_System.Domain.Contracts
{
    public interface IStudyYearRepository : IGenericRepository<StudyYear, int>
    {
        // Get by specific criteria
        Task<StudyYear?> GetCurrentStudyYearAsync();
        
        // Check existence
        Task<bool> HasRegistrationsAsync(int studyYearId);
        Task<bool> HasSemestersAsync(int studyYearId);
        Task<bool> HasFeesAsync(int studyYearId);
        Task<bool> StudyYearExistsAsync(int startYear, int endYear);
        Task<bool> IsStudyYearCurrentAsync(int studyYearId);
        
        // Counts
        Task<int> GetStudentCountAsync(int studyYearId);
        Task<int> GetSemesterCountAsync(int studyYearId);
        Task<int> GetRegistrationCountAsync(int studyYearId);
        
        // Bulk operations
        Task<IEnumerable<StudyYear>> GetStudyYearsWithDetailsAsync(CancellationToken cancellationToken = default);
        Task<(IEnumerable<StudyYear> Data, int TotalCount)> GetAllFilteredAsync(StudyYearFilterQueries query, CancellationToken cancellationToken = default);
        
        // History
        Task<IEnumerable<StudyYear>> GetPreviousStudyYearsAsync(int count);
        Task<IEnumerable<StudyYear>> GetUpcomingStudyYearsAsync(int count);

        // The statistics needed data
        Task<StudyYear?> GetStudyYearWithDetailsAsync(int studyYearId, CancellationToken cancellationToken = default);
        Task<StudyYear?> GetStudyYearWithFullDetailsAsync(int studyYearId, CancellationToken cancellationToken = default);
    }
}