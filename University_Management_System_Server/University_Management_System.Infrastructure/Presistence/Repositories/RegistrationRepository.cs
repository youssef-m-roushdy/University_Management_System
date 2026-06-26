using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Enums;
using University_Management_System.Domain.Queries.RegistrationQueries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Queries;

namespace University_Management_System.Infrastructure.Presistence.Repositories
{
    public class RegistrationRepository : GenericRepository<Registration, int>, IRegistrationRepository
    {
        public RegistrationRepository(UniversityDbContext dbContext) : base(dbContext)
        {
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY STUDENT
        // ────────────────────────────────────────────────────────────────────────

        public async Task<(IEnumerable<Registration> Data, int TotalCount)> GetByStudentIdAsync(
            string studentId,
            RegistrationFilterQueries? filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = BuildBaseQuery()
                .Where(r => r.StudentId == studentId)
                .AsQueryable();

            query = ApplyFilters(query, filter);

            return await ApplyPaginationAsync(query, filter, cancellationToken);
        }

        public async Task<(IEnumerable<Registration> Data, int TotalCount)> GetByStudentAndStudyYearAsync(
            string studentId,
            int studyYearId,
            RegistrationStudyYearQueries? filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = BuildBaseQuery()
                .Where(r => r.StudentId == studentId && r.StudyYearId == studyYearId)
                .AsQueryable();

            query = ApplyStudyYearFilters(query, filter);

            return await ApplyPaginationAsync(query, filter, cancellationToken);
        }

        public async Task<(IEnumerable<Registration> Data, int TotalCount)> GetByStudentAndStudyYearAndSemesterAsync(
            string studentId,
            int studyYearId,
            int semesterId,
            RegistrationSemesterQueries? filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = BuildBaseQuery()
                .Where(r => r.StudentId == studentId 
                            && r.StudyYearId == studyYearId 
                            && r.SemesterId == semesterId)
                .AsQueryable();

            query = ApplySemesterFilters(query, filter);

            return await ApplyPaginationAsync(query, filter, cancellationToken);
        }

        public async Task<Registration?> GetByStudentAndCourseAsync(
            string studentId,
            int courseId,
            int studyYearId)
        {
            return await BuildBaseQuery()
                .FirstOrDefaultAsync(r => r.StudentId == studentId 
                                          && r.CourseId == courseId 
                                          && r.StudyYearId == studyYearId);
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY COURSE
        // ────────────────────────────────────────────────────────────────────────

        public async Task<(IEnumerable<Registration> Data, int TotalCount)> GetByCourseIdAsync(
            int courseId,
            RegistrationFilterQueries? filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = BuildBaseQuery()
                .Where(r => r.CourseId == courseId)
                .AsQueryable();

            query = ApplyFilters(query, filter);

            return await ApplyPaginationAsync(query, filter, cancellationToken);
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY SEMESTER
        // ────────────────────────────────────────────────────────────────────────

        public async Task<(IEnumerable<Registration> Data, int TotalCount)> GetBySemesterIdAsync(
            int semesterId,
            RegistrationSemesterQueries? filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = BuildBaseQuery()
                .Where(r => r.SemesterId == semesterId)
                .AsQueryable();

            query = ApplySemesterFilters(query, filter);

            return await ApplyPaginationAsync(query, filter, cancellationToken);
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY STUDY YEAR
        // ────────────────────────────────────────────────────────────────────────

        public async Task<(IEnumerable<Registration> Data, int TotalCount)> GetByStudyYearIdAsync(
            int studyYearId,
            RegistrationStudyYearQueries? filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = BuildBaseQuery()
                .Where(r => r.StudyYearId == studyYearId)
                .AsQueryable();

            query = ApplyStudyYearFilters(query, filter);

            return await ApplyPaginationAsync(query, filter, cancellationToken);
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY IDS
        // ────────────────────────────────────────────────────────────────────────

        public async Task<IEnumerable<Registration>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await BuildBaseQuery()
                .Where(r => ids.Contains(r.Id))
                .ToListAsync();
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET ALL WITH FILTERS
        // ────────────────────────────────────────────────────────────────────────

        public async Task<(IEnumerable<Registration> Data, int TotalCount)> GetAllFilteredAsync(
            RegistrationFilterQueries? filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = BuildBaseQuery()
                .AsQueryable();

            query = ApplyFilters(query, filter);

            return await ApplyPaginationAsync(query, filter, cancellationToken);
        }

        // ────────────────────────────────────────────────────────────────────────
        // STUDENT PROGRESS
        // ────────────────────────────────────────────────────────────────────────

        public async Task<IEnumerable<Registration>> GetStudentPassedCoursesAsync(string studentId)
        {
            return await BuildBaseQuery()
                .Where(r => r.StudentId == studentId && r.IsPassed)
                .ToListAsync();
        }

        public async Task<IEnumerable<Registration>> GetStudentSemesterCoursesAsync(
            string studentId,
            int studyYearId,
            int semesterId)
        {
            return await BuildBaseQuery()
                .Where(r => r.StudentId == studentId 
                            && r.StudyYearId == studyYearId 
                            && r.SemesterId == semesterId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Registration>> GetStudentInProgressCoursesAsync(string studentId)
        {
            return await BuildBaseQuery()
                .Where(r => r.StudentId == studentId && r.Progress == CourseProgress.InProgress)
                .ToListAsync();
        }

        public async Task<IEnumerable<Registration>> GetStudentFailedCoursesAsync(string studentId)
        {
            return await BuildBaseQuery()
                .Where(r => r.StudentId == studentId && !r.IsPassed && r.Progress == CourseProgress.Completed)
                .ToListAsync();
        }

        public async Task<IEnumerable<Registration>> GetStudentCoursesByStudyYearAsync(string studentId, int studyYearId)
        {
            return await BuildBaseQuery()
                .Where(r => r.StudentId == studentId && r.StudyYearId == studyYearId)
                .ToListAsync();
        }

        // ────────────────────────────────────────────────────────────────────────
        // CHECK EXISTENCE
        // ────────────────────────────────────────────────────────────────────────

        public async Task<bool> IsStudentRegisteredInCourseAsync(string studentId, int courseId)
        {
            return await GetQueryable()
                .AnyAsync(r => r.StudentId == studentId && r.CourseId == courseId);
        }

        public async Task<bool> IsCourseCompletedByStudentAsync(string studentId, int courseId)
        {
            return await GetQueryable()
                .AnyAsync(r => r.StudentId == studentId 
                               && r.CourseId == courseId 
                               && r.IsPassed 
                               && r.Progress == CourseProgress.Completed);
        }

        public async Task<bool> IsStudentRegisteredInSemesterAsync(string studentId, int semesterId)
        {
            return await GetQueryable()
                .AnyAsync(r => r.StudentId == studentId && r.SemesterId == semesterId);
        }

        public async Task<bool> IsStudentRegisteredInStudyYearAsync(string studentId, int studyYearId)
        {
            return await GetQueryable()
                .AnyAsync(r => r.StudentId == studentId && r.StudyYearId == studyYearId);
        }

        // ────────────────────────────────────────────────────────────────────────
        // COUNTS
        // ────────────────────────────────────────────────────────────────────────

        public async Task<int> GetRegistrationCountBySemesterAsync(int semesterId)
        {
            return await GetQueryable()
                .CountAsync(r => r.SemesterId == semesterId);
        }

        public async Task<int> GetRegistrationCountByCourseAsync(int courseId)
        {
            return await GetQueryable()
                .CountAsync(r => r.CourseId == courseId);
        }

        public async Task<int> GetRegistrationCountByStudentAsync(string studentId)
        {
            return await GetQueryable()
                .CountAsync(r => r.StudentId == studentId);
        }

        public async Task<int> GetRegistrationCountByStudyYearAsync(int studyYearId)
        {
            return await GetQueryable()
                .CountAsync(r => r.StudyYearId == studyYearId);
        }

        public async Task<int> GetStudentCreditHoursAsync(string studentId, int semesterId)
        {
            var registrations = await GetQueryable()
                .Where(r => r.StudentId == studentId && r.SemesterId == semesterId)
                .Include(r => r.Course)
                .ToListAsync();

            return registrations.Sum(r => r.Course.Credits);
        }

        public async Task<int> GetStudentTotalCreditHoursAsync(string studentId)
        {
            var registrations = await GetQueryable()
                .Where(r => r.StudentId == studentId && r.IsPassed)
                .Include(r => r.Course)
                .ToListAsync();

            return registrations.Sum(r => r.Course.Credits);
        }

        // ────────────────────────────────────────────────────────────────────────
        // BULK OPERATIONS
        // ────────────────────────────────────────────────────────────────────────

        public async Task AddRangeAsync(IEnumerable<Registration> registrations)
        {
            await _dbContext.Registrations.AddRangeAsync(registrations);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(IEnumerable<Registration> registrations)
        {
            _dbContext.Registrations.UpdateRange(registrations);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<Registration> registrations)
        {
            _dbContext.Registrations.RemoveRange(registrations);
            await _dbContext.SaveChangesAsync();
        }

        // ────────────────────────────────────────────────────────────────────────
        // PRIVATE HELPERS
        // ────────────────────────────────────────────────────────────────────────

        private IQueryable<Registration> BuildBaseQuery()
        {
            return GetQueryable()
                .Include(r => r.Course)
                .Include(r => r.Student)
                    .ThenInclude(s => s.User)
                .Include(r => r.Semester)
                .Include(r => r.StudyYear)
                .AsNoTracking();
        }

        private IQueryable<Registration> ApplyFilters(
            IQueryable<Registration> query,
            RegistrationFilterQueries? filter)
        {
            if (filter == null) return query;

            if (filter.Status.HasValue)
                query = query.Where(r => r.Status == filter.Status.Value);

            if (filter.IsPassed.HasValue)
                query = query.Where(r => r.IsPassed == filter.IsPassed.Value);

            if (filter.Progress.HasValue)
                query = query.Where(r => r.Progress == filter.Progress.Value);

            if (filter.Grade.HasValue)
                query = query.Where(r => r.Grade == filter.Grade.Value);

            if (!string.IsNullOrEmpty(filter.StudentName))
                query = query.Where(r => r.Student.User.Name.Contains(filter.StudentName));

            if (!string.IsNullOrEmpty(filter.AcademicCode))
                query = query.Where(r => r.Student.AcademicCode.Contains(filter.AcademicCode));

            if (!string.IsNullOrEmpty(filter.CourseName))
                query = query.Where(r => r.Course.Name.Contains(filter.CourseName));

            if (!string.IsNullOrEmpty(filter.CourseCode))
                query = query.Where(r => r.Course.Code.Contains(filter.CourseCode));

            if (filter.RegisteredFrom.HasValue)
                query = query.Where(r => r.RegisteredAt >= filter.RegisteredFrom.Value);

            if (filter.RegisteredTo.HasValue)
                query = query.Where(r => r.RegisteredAt <= filter.RegisteredTo.Value);

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                var searchTerm = filter.SearchTerm.ToLower();
                query = query.Where(r =>
                    r.Student.User.Name.ToLower().Contains(searchTerm) ||
                    r.Student.AcademicCode.ToLower().Contains(searchTerm) ||
                    r.Course.Name.ToLower().Contains(searchTerm) ||
                    r.Course.Code.ToLower().Contains(searchTerm));
            }

            return query;
        }

        private IQueryable<Registration> ApplySemesterFilters(
            IQueryable<Registration> query,
            RegistrationSemesterQueries? filter)
        {
            if (filter == null) return query;

            if (filter.Status.HasValue)
                query = query.Where(r => r.Status == filter.Status.Value);

            if (filter.IsPassed.HasValue)
                query = query.Where(r => r.IsPassed == filter.IsPassed.Value);

            if (filter.Progress.HasValue)
                query = query.Where(r => r.Progress == filter.Progress.Value);

            if (filter.Grade.HasValue)
                query = query.Where(r => r.Grade == filter.Grade.Value);

            if (!string.IsNullOrEmpty(filter.StudentName))
                query = query.Where(r => r.Student.User.Name.Contains(filter.StudentName));

            if (!string.IsNullOrEmpty(filter.AcademicCode))
                query = query.Where(r => r.Student.AcademicCode.Contains(filter.AcademicCode));

            if (!string.IsNullOrEmpty(filter.CourseName))
                query = query.Where(r => r.Course.Name.Contains(filter.CourseName));

            if (!string.IsNullOrEmpty(filter.CourseCode))
                query = query.Where(r => r.Course.Code.Contains(filter.CourseCode));

            if (filter.RegisteredFrom.HasValue)
                query = query.Where(r => r.RegisteredAt >= filter.RegisteredFrom.Value);

            if (filter.RegisteredTo.HasValue)
                query = query.Where(r => r.RegisteredAt <= filter.RegisteredTo.Value);

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                var searchTerm = filter.SearchTerm.ToLower();
                query = query.Where(r =>
                    r.Student.User.Name.ToLower().Contains(searchTerm) ||
                    r.Student.AcademicCode.ToLower().Contains(searchTerm) ||
                    r.Course.Name.ToLower().Contains(searchTerm) ||
                    r.Course.Code.ToLower().Contains(searchTerm));
            }

            return query;
        }

        private IQueryable<Registration> ApplyStudyYearFilters(
            IQueryable<Registration> query,
            RegistrationStudyYearQueries? filter)
        {
            if (filter == null) return query;

            if (filter.SemesterTitle.HasValue)
                query = query.Where(r => r.Semester.Title == filter.SemesterTitle.Value);

            if (filter.Status.HasValue)
                query = query.Where(r => r.Status == filter.Status.Value);

            if (filter.IsPassed.HasValue)
                query = query.Where(r => r.IsPassed == filter.IsPassed.Value);

            if (filter.Progress.HasValue)
                query = query.Where(r => r.Progress == filter.Progress.Value);

            if (filter.Grade.HasValue)
                query = query.Where(r => r.Grade == filter.Grade.Value);

            if (!string.IsNullOrEmpty(filter.StudentName))
                query = query.Where(r => r.Student.User.Name.Contains(filter.StudentName));

            if (!string.IsNullOrEmpty(filter.AcademicCode))
                query = query.Where(r => r.Student.AcademicCode.Contains(filter.AcademicCode));

            if (!string.IsNullOrEmpty(filter.CourseName))
                query = query.Where(r => r.Course.Name.Contains(filter.CourseName));

            if (!string.IsNullOrEmpty(filter.CourseCode))
                query = query.Where(r => r.Course.Code.Contains(filter.CourseCode));

            if (filter.RegisteredFrom.HasValue)
                query = query.Where(r => r.RegisteredAt >= filter.RegisteredFrom.Value);

            if (filter.RegisteredTo.HasValue)
                query = query.Where(r => r.RegisteredAt <= filter.RegisteredTo.Value);

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                var searchTerm = filter.SearchTerm.ToLower();
                query = query.Where(r =>
                    r.Student.User.Name.ToLower().Contains(searchTerm) ||
                    r.Student.AcademicCode.ToLower().Contains(searchTerm) ||
                    r.Course.Name.ToLower().Contains(searchTerm) ||
                    r.Course.Code.ToLower().Contains(searchTerm));
            }

            return query;
        }

        private async Task<(IEnumerable<Registration> Data, int TotalCount)> ApplyPaginationAsync(
            IQueryable<Registration> query,
            SearchablePaginationQuery? filter,
            CancellationToken cancellationToken)
        {
            var totalCount = await query.CountAsync(cancellationToken);

            var pageNumber = filter?.PageNumber ?? 1;
            var pageSize = filter?.PageSize ?? 10;

            // Apply sorting
            query = filter?.SortBy?.ToLower() switch
            {
                "student" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(r => r.Student.User.Name)
                    : query.OrderByDescending(r => r.Student.User.Name),
                "course" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(r => r.Course.Name)
                    : query.OrderByDescending(r => r.Course.Name),
                "semester" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(r => r.Semester.Title)
                    : query.OrderByDescending(r => r.Semester.Title),
                "status" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(r => r.Status)
                    : query.OrderByDescending(r => r.Status),
                "progress" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(r => r.Progress)
                    : query.OrderByDescending(r => r.Progress),
                "grade" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(r => r.Grade)
                    : query.OrderByDescending(r => r.Grade),
                "registeredat" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(r => r.RegisteredAt)
                    : query.OrderByDescending(r => r.RegisteredAt),
                _ => query.OrderBy(r => r.RegisteredAt)
            };

            var result = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (result, totalCount);
        }
    }
}