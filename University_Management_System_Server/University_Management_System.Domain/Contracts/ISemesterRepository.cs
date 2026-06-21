using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Queries;

namespace University_Management_System.Domain.Contracts
{
    public interface ISemesterRepository : IGenericRepository<Semester, int>
    {
        public Task<IEnumerable<Semester>> GetByStudyYearIdAsync(int studyYearId);
        Task<Semester?> GetActiveSemesterByStudyYearIdAsync(int studyYearId);
        Task<bool> IsActiveSemesterAsync(int semesterId);
        Task<bool> IsSemesterBelongsToStudyYearAsync(int semesterId, int studyYearId);
        Task<(IEnumerable<Semester> Data, int TotalCount)> GetByStudyYearIdAsync(
            int studyYearId,
            GetStudyYearNestedQueries query,
            CancellationToken cancellationToken);
    }
}