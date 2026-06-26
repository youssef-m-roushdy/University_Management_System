using System.Collections.Generic;
using System.Threading.Tasks;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Contracts
{
    public interface IGpaCalculationService
    {
        decimal GetGradePoint(Grades grade);

        Task<SemesterGPA?> CalculateAndSaveSemesterGPAAsync(string userId, int semesterId, int studyYearId);

        Task<SemesterGPA?> GetSemesterGPAAsync(string userId, int semesterId, int studyYearId);

        Task<IEnumerable<SemesterGPA>> GetAllStudentGPAsAsync(string userId);

        Task<decimal> CalculateCumulativeGPAAsync(string userId);

        /// <summary>
        /// Recalculates a student's cumulative GPA from all SemesterGPA records
        /// and persists it on Student.TotalGPA.
        /// </summary>
        Task UpdateStudentCumulativeGpaAsync(string studentId);

        /// <summary>
        /// Given registrations that just had Grade/IsPassed updated (single or bulk),
        /// recalculates SemesterGPA for every distinct (Student, Semester, StudyYear)
        /// they belong to, then refreshes each affected student's cumulative GPA.
        /// Call this AFTER the registration changes are saved to the database.
        /// </summary>
        Task RecalculateGpaForRegistrationsAsync(IEnumerable<Registration> updatedRegistrations);
    }
}