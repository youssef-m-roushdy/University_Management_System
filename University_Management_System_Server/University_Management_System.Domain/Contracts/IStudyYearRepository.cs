using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Domain.Contracts
{
    public interface IStudyYearRepository : IGenericRepository<StudyYear, int>
    {
        Task<StudyYear?> GetCurrentStudyYearAsync();
        Task<bool> IsCurrentStudyYearAsync(int studyYearId);
    }
}
