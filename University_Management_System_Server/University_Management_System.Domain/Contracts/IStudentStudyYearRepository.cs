using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Queries;

namespace University_Management_System.Domain.Contracts
{
    public interface IStudentStudyYearRepository : IGenericRepository<StudentStudyYear, int>
    {
        Task<IEnumerable<StudentStudyYear>> GetByStudentIdAsync(string studentId);
        Task<StudentStudyYear?> GetCurrentByStudentIdAsync(string studentId);
        Task<StudentStudyYear?> GetByStudentAndStudyYearAsync(string studentId, int studyYearId);
        Task<IEnumerable<StudentStudyYear>> GetByStudyYearIdAsync(int studyYearId);
        Task AddRangeAsync(IEnumerable<StudentStudyYear> StudentStudyYears);
        Task<IEnumerable<StudentStudyYear>> GetStudyYearsByStudentIdAsync(string studentId);

        Task<(IEnumerable<StudentStudyYear> Data, int TotalCount)> GetStudentsOfTheStudyYearByIdAsync(
            int studyYearId,
            StudyYearStudentQueries query,
            CancellationToken cancellationToken);
    }
}
