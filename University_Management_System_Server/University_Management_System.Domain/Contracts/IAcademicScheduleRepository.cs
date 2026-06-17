using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Domain.Contracts
{
    public interface IAcademicScheduleRepository : IGenericRepository<AcademicSchedule, int>
    {
        Task<AcademicSchedule?> GetByTitleAsync(string title);
        Task<IEnumerable<AcademicSchedule>> GetAllWithDetailsAsync();
        Task<AcademicSchedule?> UploadSemesterAcademicScheduleAsync(AcademicSchedule schedule);
        Task<AcademicSchedule?> GetBySemesterIdAsync(int semesterId);
    }
}
