using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Queries.RegistrationQueries;

namespace University_Management_System.Domain.Contracts
{
    public interface IRegistrationRepository : IGenericRepository<Registration, int>
    {
        // ─── Get by student ────────────────────────────────────────────────────
        Task<(IEnumerable<Registration> Data, int TotalCount)> GetByStudentIdAsync(string studentId, RegistrationFilterQueries? filter = null, CancellationToken cancellationToken = default);
        Task<(IEnumerable<Registration> Data, int TotalCount)> GetByStudentAndStudyYearAsync(string studentId, int studyYearId, RegistrationStudyYearQueries? filter = null, CancellationToken cancellationToken = default);
        Task<(IEnumerable<Registration> Data, int TotalCount)> GetByStudentAndStudyYearAndSemesterAsync(string studentId, int studyYearId, int semesterId, RegistrationSemesterQueries? filter = null, CancellationToken cancellationToken = default);
        Task<Registration?> GetByStudentAndCourseAsync(string studentId, int courseId, int studyYearId);

        // ─── Get by course ────────────────────────────────────────────────────
        Task<(IEnumerable<Registration> Data, int TotalCount)> GetByCourseIdAsync(int courseId, RegistrationFilterQueries? filter = null, CancellationToken cancellationToken = default);

        // ─── Get by semester ──────────────────────────────────────────────────
        Task<(IEnumerable<Registration> Data, int TotalCount)> GetBySemesterIdAsync(int semesterId, RegistrationSemesterQueries? filter = null, CancellationToken cancellationToken = default);

        // ─── Get by study year ───────────────────────────────────
        Task<(IEnumerable<Registration> Data, int TotalCount)> GetByStudyYearIdAsync(
            int studyYearId,
            RegistrationStudyYearQueries? filter = null,
            CancellationToken cancellationToken = default);

        // ─── Get all with filters ─────────────────────────────────────────────
        Task<(IEnumerable<Registration> Data, int TotalCount)> GetAllFilteredAsync(RegistrationFilterQueries? filter = null, CancellationToken cancellationToken = default);

        // ─── Student progress ──────────────────────────────────────────────────
        Task<IEnumerable<Registration>> GetStudentPassedCoursesAsync(string studentId);
        Task<IEnumerable<Registration>> GetStudentSemesterCoursesAsync(string studentId, int studyYearId, int semesterId);
        Task<IEnumerable<Registration>> GetStudentInProgressCoursesAsync(string studentId);
        Task<IEnumerable<Registration>> GetStudentFailedCoursesAsync(string studentId);
        Task<IEnumerable<Registration>> GetStudentCoursesByStudyYearAsync(string studentId, int studyYearId);

        // ─── Check existence ──────────────────────────────────────────────────
        Task<bool> IsStudentRegisteredInCourseAsync(string studentId, int courseId);
        Task<bool> IsCourseCompletedByStudentAsync(string studentId, int courseId);
        Task<bool> IsStudentRegisteredInSemesterAsync(string studentId, int semesterId);
        Task<bool> IsStudentRegisteredInStudyYearAsync(string studentId, int studyYearId);

        // ─── Counts ────────────────────────────────────────────────────────────
        Task<int> GetRegistrationCountBySemesterAsync(int semesterId);
        Task<int> GetRegistrationCountByCourseAsync(int courseId);
        Task<int> GetRegistrationCountByStudentAsync(string studentId);
        Task<int> GetRegistrationCountByStudyYearAsync(int studyYearId);
        Task<int> GetStudentCreditHoursAsync(string studentId, int semesterId);
        Task<int> GetStudentTotalCreditHoursAsync(string studentId);

        // ─── Bulk operations ──────────────────────────────────────────────────
        Task AddRangeAsync(IEnumerable<Registration> registrations);
        Task UpdateRangeAsync(IEnumerable<Registration> registrations);
        Task DeleteRangeAsync(IEnumerable<Registration> registrations);

        /// <summary>
        /// Fetches multiple registrations by Id in a single round-trip.
        /// Used by the bulk grade-patch flow.
        /// </summary>
        // ─── Get by multiple IDs ──────────────────────────────────────────────────
        Task<IEnumerable<Registration>> GetByIdsAsync(IEnumerable<int> ids);

    }
}