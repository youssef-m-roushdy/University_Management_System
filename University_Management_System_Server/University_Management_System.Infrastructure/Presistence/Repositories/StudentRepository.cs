using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Queries.StudentQueries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Enums;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Domain.Queries;

namespace University_Management_System.Infrastructure.Presistence.Repositories
{
    public class StudentRepository : GenericRepository<Student, string>, IStudentRepository
    {
        public StudentRepository(UniversityDbContext dbContext) : base(dbContext)
        {
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY CRITERIA
        // ────────────────────────────────────────────────────────────────────────

        public async Task<Student?> GetStudentByUserIdAsync(string userId)
        {
            return await _dbContext.Students
                .Include(s => s.User)
                .Include(s => s.Department)
                .Include(s => s.Specialization)
                .FirstOrDefaultAsync(s => s.Id == userId);
        }

        public async Task<Student?> GetStudentByAcademicCodeAsync(string academicCode)
        {
            return await _dbContext.Students
                .FirstOrDefaultAsync(s => s.AcademicCode == academicCode);
        }

        public async Task<Student?> GetStudentByAcademicCodeWithDetailsAsync(string academicCode)
        {
            return await _dbContext.Students
                .Include(s => s.User)
                .Include(s => s.Department)
                .Include(s => s.Specialization)
                .Include(s => s.StudentStudyYears)
                    .ThenInclude(ssy => ssy.StudyYear)
                .FirstOrDefaultAsync(s => s.AcademicCode == academicCode);
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET ALL WITH FILTERS AND PAGINATION
        // ────────────────────────────────────────────────────────────────────────

        public async Task<(IEnumerable<Student> Data, int TotalCount)> GetAllFilteredAsync(
            StudentFilterQueries query,
            CancellationToken cancellationToken = default)
        {
            var queryable = _dbContext.Students
                .Include(s => s.User)
                .Include(s => s.Department)
                .Include(s => s.Specialization)
                .AsNoTracking()
                .AsQueryable();

            // ─── Level Filter ──────────────────────────────────────────────────
            if (query.Level.HasValue)
                queryable = queryable.Where(s => s.Level == query.Level.Value);

            // ─── Gender Filter ─────────────────────────────────────────────────
            if (query.Gender.HasValue)
                queryable = queryable.Where(s => s.User.Gender == query.Gender.Value);

            // ─── Department Search ────────────────────────────────────────────
            if (!string.IsNullOrEmpty(query.DepartmentSearch))
            {
                var searchTerm = query.DepartmentSearch.ToLower();
                queryable = queryable.Where(s =>
                    s.Department != null &&
                    (s.Department.Name.ToLower().Contains(searchTerm) ||
                     s.Department.Code.ToLower().Contains(searchTerm)));
            }

            // ─── Specialization Search ────────────────────────────────────────
            if (!string.IsNullOrEmpty(query.SpecializationSearch))
            {
                var searchTerm = query.SpecializationSearch.ToLower();
                queryable = queryable.Where(s =>
                    s.Specialization != null &&
                    s.Specialization.Name.ToLower().Contains(searchTerm));
            }

            // ─── GPA Range ────────────────────────────────────────────────────
            if (query.MinGPA.HasValue)
                queryable = queryable.Where(s => s.TotalGPA >= query.MinGPA.Value);

            if (query.MaxGPA.HasValue)
                queryable = queryable.Where(s => s.TotalGPA <= query.MaxGPA.Value);

            // ─── Total Credits Range ──────────────────────────────────────────
            if (query.MinTotalCredits.HasValue)
                queryable = queryable.Where(s => s.TotalCredits >= query.MinTotalCredits.Value);

            if (query.MaxTotalCredits.HasValue)
                queryable = queryable.Where(s => s.TotalCredits <= query.MaxTotalCredits.Value);

            // ─── Allowed Credits Range ──────────────────────────────────────
            if (query.MinAllowedCredits.HasValue)
                queryable = queryable.Where(s => s.AllowedCredits >= query.MinAllowedCredits.Value);

            if (query.MaxAllowedCredits.HasValue)
                queryable = queryable.Where(s => s.AllowedCredits <= query.MaxAllowedCredits.Value);

            // ─── Graduation Status ────────────────────────────────────────────
            if (query.IsGraduated.HasValue)
            {
                if (query.IsGraduated.Value)
                    queryable = queryable.Where(s => s.Level == Levels.Graduate);
                else
                    queryable = queryable.Where(s => s.Level != Levels.Graduate);
            }

            // ─── Active Status ─────────────────────────────────────────────────
            if (query.IsActive.HasValue)
                queryable = queryable.Where(s => s.User.IsActive == query.IsActive.Value);

            // ─── Search Term ──────────────────────────────────────────────────
            // Inherited from SearchablePaginationQuery
            // Searches: Name, AcademicCode, Email
            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                var searchTerm = query.SearchTerm.ToLower();
                queryable = queryable.Where(s =>
                    s.User.Name.ToLower().Contains(searchTerm) ||
                    s.AcademicCode.ToLower().Contains(searchTerm) ||
                    s.User.Email.ToLower().Contains(searchTerm));
            }

            // ─── Get Total Count ────────────────────────────────────────────
            var totalCount = await queryable.CountAsync(cancellationToken);

            // ─── Apply Sorting ──────────────────────────────────────────────
            queryable = query.SortBy?.ToLower() switch
            {
                "name" => query.SortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(s => s.User.Name)
                    : queryable.OrderByDescending(s => s.User.Name),
                "academiccode" => query.SortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(s => s.AcademicCode)
                    : queryable.OrderByDescending(s => s.AcademicCode),
                "level" => query.SortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(s => s.Level)
                    : queryable.OrderByDescending(s => s.Level),
                "gpa" => query.SortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(s => s.TotalGPA)
                    : queryable.OrderByDescending(s => s.TotalGPA),
                "totalcredits" => query.SortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(s => s.TotalCredits)
                    : queryable.OrderByDescending(s => s.TotalCredits),
                "department" => query.SortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(s => s.Department.Name)
                    : queryable.OrderByDescending(s => s.Department.Name),
                _ => queryable.OrderBy(s => s.User.Name)
            };

            // ─── Apply Pagination ────────────────────────────────────────────
            var result = await queryable
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);

            return (result, totalCount);
        }

        public async Task<(IEnumerable<Student> Data, int TotalCount)> GetDepartmentStudentsAsync(
            int departmentId,
            StudentDepartmentQueries query,
            CancellationToken cancellationToken = default)
        {
            var queryable = _dbContext.Students
                .Include(s => s.User)
                .Include(s => s.Department)
                .Include(s => s.Specialization)
                .Where(s => s.DepartmentId == departmentId)  // ✅ Fixed department
                .AsNoTracking()
                .AsQueryable();

            // ─── Level Filter ──────────────────────────────────────────────────
            if (query.Level.HasValue)
                queryable = queryable.Where(s => s.Level == query.Level.Value);

            // ─── Gender Filter ─────────────────────────────────────────────────
            if (query.Gender.HasValue)
                queryable = queryable.Where(s => s.User.Gender == query.Gender.Value);

            // ─── Specialization Search ────────────────────────────────────────
            if (!string.IsNullOrEmpty(query.SpecializationSearch))
            {
                var searchTerm = query.SpecializationSearch.ToLower();
                queryable = queryable.Where(s =>
                    s.Specialization != null &&
                    s.Specialization.Name.ToLower().Contains(searchTerm));
            }

            // ─── GPA Range ────────────────────────────────────────────────────
            if (query.MinGPA.HasValue)
                queryable = queryable.Where(s => s.TotalGPA >= query.MinGPA.Value);

            if (query.MaxGPA.HasValue)
                queryable = queryable.Where(s => s.TotalGPA <= query.MaxGPA.Value);

            // ─── Total Credits Range ──────────────────────────────────────────
            if (query.MinTotalCredits.HasValue)
                queryable = queryable.Where(s => s.TotalCredits >= query.MinTotalCredits.Value);

            if (query.MaxTotalCredits.HasValue)
                queryable = queryable.Where(s => s.TotalCredits <= query.MaxTotalCredits.Value);

            // ─── Allowed Credits Range ──────────────────────────────────────
            if (query.MinAllowedCredits.HasValue)
                queryable = queryable.Where(s => s.AllowedCredits >= query.MinAllowedCredits.Value);

            if (query.MaxAllowedCredits.HasValue)
                queryable = queryable.Where(s => s.AllowedCredits <= query.MaxAllowedCredits.Value);

            // ─── Graduation Status ────────────────────────────────────────────
            if (query.IsGraduated.HasValue)
            {
                if (query.IsGraduated.Value)
                    queryable = queryable.Where(s => s.Level == Levels.Graduate);
                else
                    queryable = queryable.Where(s => s.Level != Levels.Graduate);
            }

            // ─── Active Status ─────────────────────────────────────────────────
            if (query.IsActive.HasValue)
                queryable = queryable.Where(s => s.User.IsActive == query.IsActive.Value);

            // ─── Search Term ──────────────────────────────────────────────────
            // Inherited from SearchablePaginationQuery
            // Searches: Name, AcademicCode, Email
            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                var searchTerm = query.SearchTerm.ToLower();
                queryable = queryable.Where(s =>
                    s.User.Name.ToLower().Contains(searchTerm) ||
                    s.AcademicCode.ToLower().Contains(searchTerm) ||
                    s.User.Email.ToLower().Contains(searchTerm));
            }

            // ─── Get Total Count ────────────────────────────────────────────
            var totalCount = await queryable.CountAsync(cancellationToken);

            // ─── Apply Sorting ──────────────────────────────────────────────
            queryable = query.SortBy?.ToLower() switch
            {
                "name" => query.SortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(s => s.User.Name)
                    : queryable.OrderByDescending(s => s.User.Name),
                "academiccode" => query.SortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(s => s.AcademicCode)
                    : queryable.OrderByDescending(s => s.AcademicCode),
                "level" => query.SortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(s => s.Level)
                    : queryable.OrderByDescending(s => s.Level),
                "gpa" => query.SortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(s => s.TotalGPA)
                    : queryable.OrderByDescending(s => s.TotalGPA),
                "totalcredits" => query.SortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(s => s.TotalCredits)
                    : queryable.OrderByDescending(s => s.TotalCredits),
                "specialization" => query.SortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(s => s.Specialization.Name)
                    : queryable.OrderByDescending(s => s.Specialization.Name),
                _ => queryable.OrderBy(s => s.User.Name)
            };

            // ─── Apply Pagination ────────────────────────────────────────────
            var result = await queryable
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);

            return (result, totalCount);
        }

        // ────────────────────────────────────────────────────────────────────────
        // COUNTS
        // ────────────────────────────────────────────────────────────────────────

        public async Task<int> GetStudentCountByLevelAsync(Levels level)
        {
            return await _dbContext.Students
                .CountAsync(s => s.Level == level);
        }

        public async Task<int> GetStudentCountByDepartmentAsync(int departmentId)
        {
            return await _dbContext.Students
                .CountAsync(s => s.DepartmentId == departmentId);
        }

        public async Task<int> GetStudentCountBySpecializationAsync(int specializationId)
        {
            return await _dbContext.Students
                .CountAsync(s => s.SpecializationId == specializationId);
        }

        // ────────────────────────────────────────────────────────────────────────
        // STATISTICS
        // ────────────────────────────────────────────────────────────────────────

        public async Task<decimal> GetAverageGPAAsync()
        {
            return await _dbContext.Students
                .Where(s => s.TotalGPA > 0)
                .AverageAsync(s => s.TotalGPA);
        }

        public async Task<decimal> GetAverageGPAByDepartmentAsync(int departmentId)
        {
            return await _dbContext.Students
                .Where(s => s.DepartmentId == departmentId && s.TotalGPA > 0)
                .AverageAsync(s => s.TotalGPA);
        }

        public async Task<decimal> GetAverageGPAByLevelAsync(Levels level)
        {
            return await _dbContext.Students
                .Where(s => s.Level == level && s.TotalGPA > 0)
                .AverageAsync(s => s.TotalGPA);
        }

        // ────────────────────────────────────────────────────────────────────────
        // CHECK EXISTENCE
        // ────────────────────────────────────────────────────────────────────────

        public async Task<bool> StudentExistsAsync(string studentId)
        {
            return await _dbContext.Students
                .AnyAsync(s => s.Id == studentId);
        }

        public async Task<bool> AcademicCodeExistsAsync(string academicCode)
        {
            return await _dbContext.Students
                .AnyAsync(s => s.AcademicCode == academicCode);
        }

        public async Task<bool> IsStudentActiveAsync(string studentId)
        {
            return await _dbContext.Students
                .AnyAsync(s => s.Id == studentId && s.User.IsActive);
        }

        // ────────────────────────────────────────────────────────────────────────
        // LEVEL MANAGEMENT
        // ────────────────────────────────────────────────────────────────────────

        public async Task<Levels?> GetLastLevelByStudentIdAsync(string studentId)
        {
            var lastEnrollment = await _dbContext.StudentStudyYears
                .Where(ssy => ssy.StudentId == studentId)
                .OrderByDescending(ssy => ssy.StudyYear.StartYear)
                .FirstOrDefaultAsync();

            return (Levels)lastEnrollment?.Level;
        }

        public Task<Student> UpdateStudentAllowedCredits(string studentId, int allowedCredits)
        {
            throw new NotImplementedException();
        }

        public Task<Student> UpdateStudentTotalCredits(string studentId, int totalCredits)
        {
            throw new NotImplementedException();
        }

        public Task<Student> UpdateStudentTotalGpa(string studentId, decimal totalGpa)
        {
            throw new NotImplementedException();
        }

        public Task<Student> UpdateStudentLevel(string studentId, Levels level)
        {
            throw new NotImplementedException();
        }

        public Task<Student> UpdateStudentSpecialization(string studentId, int specializationId)
        {
            throw new NotImplementedException();
        }
    }
}