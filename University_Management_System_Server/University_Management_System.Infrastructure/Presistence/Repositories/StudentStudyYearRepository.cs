using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Enums;
using University_Management_System.Domain.Queries;

namespace University_Management_System.Infrastructure.Presistence.Repositories
{
    public class StudentStudyYearRepository : GenericRepository<StudentStudyYear, int>, IStudentStudyYearRepository
    {
        public StudentStudyYearRepository(UniversityDbContext dbContext) : base(dbContext)
        {
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY STUDENT
        // ────────────────────────────────────────────────────────────────────────

        public async Task<IEnumerable<StudentStudyYear>> GetByStudentIdAsync(string studentId)
        {
            return await _dbContext.StudentStudyYears
                .Include(ssy => ssy.StudyYear)
                .Where(ssy => ssy.StudentId == studentId)
                .OrderByDescending(ssy => ssy.StudyYear.StartYear)
                .ToListAsync();
        }

        public async Task<IEnumerable<StudentStudyYear>> GetActiveByStudentIdAsync(string studentId)
        {
            return await _dbContext.StudentStudyYears
                .Include(ssy => ssy.StudyYear)
                .Where(ssy => ssy.StudentId == studentId && ssy.IsActive)
                .OrderByDescending(ssy => ssy.StudyYear.StartYear)
                .ToListAsync();
        }

        public async Task<StudentStudyYear?> GetCurrentByStudentIdAsync(string studentId)
        {
            return await _dbContext.StudentStudyYears
                .Include(ssy => ssy.StudyYear)
                .FirstOrDefaultAsync(ssy => ssy.StudentId == studentId && ssy.IsActive);
        }

        public async Task<IEnumerable<StudentStudyYear>> GetStudyYearsByStudentIdAsync(string studentId)
        {
            return await _dbContext.StudentStudyYears
                .Include(ssy => ssy.StudyYear)
                .Where(ssy => ssy.StudentId == studentId)
                .OrderByDescending(ssy => ssy.StudyYear.StartYear)
                .ToListAsync();
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY STUDY YEAR
        // ────────────────────────────────────────────────────────────────────────

        public async Task<IEnumerable<StudentStudyYear>> GetByStudyYearIdAsync(int studyYearId)
        {
            return await _dbContext.StudentStudyYears
                .Include(ssy => ssy.Student)
                    .ThenInclude(s => s.User)
                .Where(ssy => ssy.StudyYearId == studyYearId)
                .OrderBy(ssy => ssy.Student.User.Name)
                .ToListAsync();
        }

        public async Task<(IEnumerable<StudentStudyYear> Data, int TotalCount)> GetStudyYearStudentsByStudyYearIdAsync(
            int studyYearId,
            StudyYearStudentQueries query,
            CancellationToken cancellationToken = default)
        {
            var studentStudyYears = GetQueryable()
                .Include(ssy => ssy.Student)
                    .ThenInclude(s => s.User)
                .Include(ssy => ssy.Student)
                    .ThenInclude(s => s.Department)
                .Include(ssy => ssy.Student)
                    .ThenInclude(s => s.Specialization)
                .Where(ssy => ssy.StudyYearId == studyYearId);

            // Apply filters
            if (query.IsActive.HasValue)
                studentStudyYears = studentStudyYears.Where(ssy => ssy.IsActive == query.IsActive.Value);

            if (query.Level.HasValue)
                studentStudyYears = studentStudyYears.Where(ssy => ssy.Level == query.Level.Value);

            if (query.MinGPA.HasValue)
                studentStudyYears = studentStudyYears.Where(ssy => ssy.Student.TotalGPA >= query.MinGPA.Value);

            if (query.MaxGPA.HasValue)
                studentStudyYears = studentStudyYears.Where(ssy => ssy.Student.TotalGPA <= query.MaxGPA.Value);

            if (query.EnrolledFrom.HasValue)
                studentStudyYears = studentStudyYears.Where(ssy => ssy.EnrolledAt >= query.EnrolledFrom.Value);

            if (query.EnrolledTo.HasValue)
                studentStudyYears = studentStudyYears.Where(ssy => ssy.EnrolledAt <= query.EnrolledTo.Value);

            // Apply search
            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                var searchTerm = query.SearchTerm.ToLower();
                studentStudyYears = studentStudyYears.Where(ssy =>
                    ssy.Student.User.Name.ToLower().Contains(searchTerm) ||
                    ssy.Student.AcademicCode.ToLower().Contains(searchTerm) ||
                    ssy.Student.User.Email.ToLower().Contains(searchTerm));
            }

            var totalCount = await studentStudyYears.CountAsync(cancellationToken);

            // Apply sorting
            studentStudyYears = query.SortBy?.ToLower() switch
            {
                "name" => query.SortDirection == SortDirection.Ascending
                    ? studentStudyYears.OrderBy(ssy => ssy.Student.User.Name)
                    : studentStudyYears.OrderByDescending(ssy => ssy.Student.User.Name),
                "academiccode" => query.SortDirection == SortDirection.Ascending
                    ? studentStudyYears.OrderBy(ssy => ssy.Student.AcademicCode)
                    : studentStudyYears.OrderByDescending(ssy => ssy.Student.AcademicCode),
                "level" => query.SortDirection == SortDirection.Ascending
                    ? studentStudyYears.OrderBy(ssy => ssy.Level)
                    : studentStudyYears.OrderByDescending(ssy => ssy.Level),
                "gpa" => query.SortDirection == SortDirection.Ascending
                    ? studentStudyYears.OrderBy(ssy => ssy.Student.TotalGPA)
                    : studentStudyYears.OrderByDescending(ssy => ssy.Student.TotalGPA),
                "enrolledat" => query.SortDirection == SortDirection.Ascending
                    ? studentStudyYears.OrderBy(ssy => ssy.EnrolledAt)
                    : studentStudyYears.OrderByDescending(ssy => ssy.EnrolledAt),
                "department" => query.SortDirection == SortDirection.Ascending
                    ? studentStudyYears.OrderBy(ssy => ssy.Student.Department.Name)
                    : studentStudyYears.OrderByDescending(ssy => ssy.Student.Department.Name),
                _ => studentStudyYears.OrderBy(ssy => ssy.Student.User.Name)
            };

            // Apply pagination
            var result = await studentStudyYears
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);

            return (result, totalCount);
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY BOTH
        // ────────────────────────────────────────────────────────────────────────

        public async Task<StudentStudyYear?> GetByStudentAndStudyYearAsync(string studentId, int studyYearId)
        {
            return await _dbContext.StudentStudyYears
                .Include(ssy => ssy.StudyYear)
                .Include(ssy => ssy.Student)
                    .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(ssy => ssy.StudentId == studentId && ssy.StudyYearId == studyYearId);
        }

        // ────────────────────────────────────────────────────────────────────────
        // BULK OPERATIONS
        // ────────────────────────────────────────────────────────────────────────

        public async Task AddRangeAsync(IEnumerable<StudentStudyYear> studentStudyYears)
        {
            await _dbContext.StudentStudyYears.AddRangeAsync(studentStudyYears);
            await _dbContext.SaveChangesAsync();
        }

        // ────────────────────────────────────────────────────────────────────────
        // CHECK EXISTENCE
        // ────────────────────────────────────────────────────────────────────────

        public async Task<bool> IsStudentEnrolledAsync(string studentId, int studyYearId)
        {
            return await _dbContext.StudentStudyYears
                .AnyAsync(ssy => ssy.StudentId == studentId && ssy.StudyYearId == studyYearId);
        }

        public async Task<bool> IsStudentActiveAsync(string studentId, int studyYearId)
        {
            return await _dbContext.StudentStudyYears
                .AnyAsync(ssy => ssy.StudentId == studentId && ssy.StudyYearId == studyYearId && ssy.IsActive);
        }

        // ────────────────────────────────────────────────────────────────────────
        // COUNTS
        // ────────────────────────────────────────────────────────────────────────

        public async Task<int> GetStudentCountByStudyYearAsync(int studyYearId)
        {
            return await _dbContext.StudentStudyYears
                .Where(ssy => ssy.StudyYearId == studyYearId)
                .CountAsync();
        }

        public async Task<int> GetStudentCountByLevelAsync(int studyYearId, Levels level, CancellationToken cancellationToken = default)
        {
            return await _dbContext.StudentStudyYears
                .Where(ssy => ssy.StudyYearId == studyYearId && ssy.Level == level)
                .CountAsync(cancellationToken);
        }
    }
}