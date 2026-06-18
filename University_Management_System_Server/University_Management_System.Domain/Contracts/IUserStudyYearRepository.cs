using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Domain.Contracts
{
    public interface IStudentStudyYearRepository : IGenericRepository<StudentStudyYear, int>
    {
        Task<IEnumerable<StudentStudyYear>> GetByUserIdAsync(string userId);
        Task<StudentStudyYear?> GetCurrentByUserIdAsync(string userId);
        Task<StudentStudyYear?> GetByUserAndStudyYearAsync(string userId, int studyYearId);
        Task<IEnumerable<StudentStudyYear>> GetByStudyYearIdAsync(int studyYearId);
        Task AddRangeAsync(IEnumerable<StudentStudyYear> StudentStudyYears);
        Task<IEnumerable<StudentStudyYear>> GetStudyYearsByUserIdAsync(string userId);
    }
}
