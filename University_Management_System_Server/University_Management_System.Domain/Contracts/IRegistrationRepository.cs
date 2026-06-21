using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Queries;

namespace University_Management_System.Domain.Contracts
{
    public interface IRegistrationRepository : IGenericRepository<Registration, int>
    {
        Task<IEnumerable<Registration>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Registration>> GetByCourseIdAsync(int courseId);
        Task<Registration?> GetByUserAndCourseAsync(string userId, int courseId, int studyYearId);
        Task<IEnumerable<Registration>> GetByUserAndStudyYearAsync(string userId, int studyYearId);
        Task<IEnumerable<Registration>> GetByUserAndStudyYearAndSemseterAsync(string userId, int studyYearId, int semesterId);
        Task<IEnumerable<Registration>> GetByUserAsync(string userId, int? studyYearId = null);
        Task<IEnumerable<Registration>> GetAllAsync(int? courseId = null, int? studyYearId = null, int? semesterId = null, string? userId = null);
        Task<bool> IsUserRegisteredInCourseAsync(string userId, int courseId);

        Task<bool> IsCourseCompletedByUserAsync(string userId, int courseId);
        Task<(IEnumerable<Registration> Data, int TotalCount)> GetAllSemesterRegistrationsPaginatedAsync(
            int semesterId,
            int studyYearId,
            RegistrationQueries? registrationQuery,
            CancellationToken cancellationToken);

        Task<IEnumerable<Registration>> GetStudentPassedCoursesAsync(string userId);
       

        Task<IEnumerable<Registration>> GetStudentSemesterRegistrationCoursesAsync(string userId, int studyYearId, int semesterId);

    }
}
