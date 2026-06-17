using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Domain.Contracts
{
    public interface ISemesterRepository : IGenericRepository<Semester, int>
    {
        public Task<IEnumerable<Semester>> GetByStudyYearIdAsync(int studyYearId);
        Task<Semester?> GetActiveSemesterByStudyYearIdAsync(int studyYearId);
        Task<bool> IsActiveSemesterAsync(int semesterId);
        Task<bool> IsSemesterBelongsToStudyYearAsync(int semesterId, int studyYearId);
    }
}