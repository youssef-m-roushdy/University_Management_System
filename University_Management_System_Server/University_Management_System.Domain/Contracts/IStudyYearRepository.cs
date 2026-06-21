using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Queries;

namespace University_Management_System.Domain.Contracts
{
    public interface IStudyYearRepository : IGenericRepository<StudyYear, int>
    {
        // Get by specific criteria
        Task<StudyYear?> GetCurrentStudyYearAsync();
        Task<StudyYear?> GetStudyYearByYearRangeAsync(int startYear, int endYear);
        
        // Get collections
        Task<IEnumerable<StudyYear>> GetStudyYearsByYearRangeAsync(int startYear, int endYear);
        Task<IEnumerable<StudyYear>> GetStudyYearsWithSemestersAsync();
        Task<IEnumerable<StudyYear>> GetStudyYearsWithRegistrationsAsync();
        Task<IEnumerable<StudyYear>> GetStudyYearsBetweenYearsAsync(int minYear, int maxYear);
        
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
        Task<IEnumerable<StudyYear>> GetStudyYearsWithDetailsAsync();
        Task<IEnumerable<StudyYear>> GetFilteredStudyYearsAsync(StudyYearQueries query);
        
        // History
        Task<IEnumerable<StudyYear>> GetPreviousStudyYearsAsync(int count);
        Task<IEnumerable<StudyYear>> GetUpcomingStudyYearsAsync(int count);
    }
}