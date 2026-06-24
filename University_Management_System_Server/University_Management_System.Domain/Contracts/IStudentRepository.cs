using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Enums;
using University_Management_System.Domain.Queries.StudentQueries;

namespace University_Management_System.Domain.Contracts
{
    public interface IStudentRepository : IGenericRepository<Student, string>
    {
        // ─── Get by criteria ──────────────────────────────────────────────────
        Task<Student?> GetStudentByUserIdAsync(string userId);
        Task<Student?> GetStudentByAcademicCodeAsync(string academicCode);
        Task<Student?> GetStudentByAcademicCodeWithDetailsAsync(string academicCode);
        
        // ─── Get collections with filters and pagination ────────────────────
        Task<(IEnumerable<Student> Data, int TotalCount)> GetAllFilteredAsync(
            StudentFilterQueries query,
            CancellationToken cancellationToken = default);
        
         // ✅ NEW: Get students in a specific department
        Task<(IEnumerable<Student> Data, int TotalCount)> GetDepartmentStudentsAsync(
            int departmentId,
            StudentDepartmentQueries query,
            CancellationToken cancellationToken = default);
        
        // ─── Get counts ──────────────────────────────────────────────────────
        Task<int> GetStudentCountByLevelAsync(Levels level);
        Task<int> GetStudentCountByDepartmentAsync(int departmentId);
        Task<int> GetStudentCountBySpecializationAsync(int specializationId);
        
        // ─── Statistics ──────────────────────────────────────────────────────
        Task<decimal> GetAverageGPAAsync();
        Task<decimal> GetAverageGPAByDepartmentAsync(int departmentId);
        Task<decimal> GetAverageGPAByLevelAsync(Levels level);
        
        // ─── Check existence ──────────────────────────────────────────────────
        Task<bool> StudentExistsAsync(string studentId);
        Task<bool> AcademicCodeExistsAsync(string academicCode);
        Task<bool> IsStudentActiveAsync(string studentId);
        
        // ─── Level management ──────────────────────────────────────────────────
        Task<Levels?> GetLastLevelByStudentIdAsync(string studentId);

        // ─── Student Partial Updates ──────────────────────────────────────────────────
        Task<Student> UpdateStudentAllowedCredits (string studentId, int allowedCredits);
        Task<Student> UpdateStudentTotalCredits (string studentId, int totalCredits);
        Task<Student> UpdateStudentTotalGpa (string studentId, decimal totalGpa);
        Task<Student> UpdateStudentLevel (string studentId, Levels level);
        Task<Student> UpdateStudentSpecialization (string studentId, int specializationId);
    }
}