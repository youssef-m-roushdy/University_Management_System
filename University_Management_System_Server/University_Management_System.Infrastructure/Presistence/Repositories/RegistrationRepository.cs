using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Enums;
using University_Management_System.Domain.Queries.RegistrationQueries;
using University_Management_System.Infrastructure.Presistence.Extensions;
using Microsoft.EntityFrameworkCore;
using University_Management_System.Domain.Queries;

namespace University_Management_System.Infrastructure.Presistence.Repositories
{
    public class RegistrationRepository : GenericRepository<Registration, int>, IRegistrationRepository
    {
        public RegistrationRepository(UniversityDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Registration>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await GetQueryable()
                .Where(r => ids.Contains(r.Id))
                .Include(r => r.Course)
                .Include(r => r.Semester)
                .Include(r => r.StudyYear)
                .ToListAsync();
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY STUDENT
        // ────────────────────────────────────────────────────────────────────────

        public async Task<(IEnumerable<Registration> Data, int TotalCount)> GetByStudentIdAsync(
            string studentId,
            RegistrationFilterQueries? filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Registrations
                .Where(r => r.StudentId == studentId)
                .Include(r => r.Course)
                .Include(r => r.StudyYear)
                .Include(r => r.Semester)
                .AsNoTracking()
                .AsQueryable();

            query = ApplyStudentFilters(query, filter);

            return await ApplyPaginationAsync(query, filter, cancellationToken);
        }

        public async Task<(IEnumerable<Registration> Data, int TotalCount)> GetByStudentAndStudyYearAsync(
            string studentId,
            int studyYearId,
            RegistrationStudyYearQueries? filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Registrations
                .Where(r => r.StudentId == studentId && r.StudyYearId == studyYearId)
                .Include(r => r.Course)
                .Include(r => r.Semester)
                .AsNoTracking()
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
            var query = _dbContext.Registrations
                .Where(r => r.StudentId == studentId && r.StudyYearId == studyYearId && r.SemesterId == semesterId)
                .Include(r => r.Course)
                .AsNoTracking()
                .AsQueryable();

            query = ApplySemesterFilters(query, filter);

            return await ApplyPaginationAsync(query, filter, cancellationToken);
        }

        public async Task<Registration?> GetByStudentAndCourseAsync(string studentId, int courseId, int studyYearId)
        {
            return await _dbContext.Registrations
                .Include(r => r.Course)
                .Include(r => r.StudyYear)
                .Include(r => r.Semester)
                .FirstOrDefaultAsync(r => r.StudentId == studentId && r.CourseId == courseId && r.StudyYearId == studyYearId);
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY COURSE
        // ────────────────────────────────────────────────────────────────────────

        public async Task<(IEnumerable<Registration> Data, int TotalCount)> GetByCourseIdAsync(
            int courseId,
            RegistrationFilterQueries? filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Registrations
                .Where(r => r.CourseId == courseId)
                .Include(r => r.Student)
                    .ThenInclude(s => s.User)
                .Include(r => r.StudyYear)
                .Include(r => r.Semester)
                .AsNoTracking()
                .AsQueryable();

            query = ApplyStudentFilters(query, filter);

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
            var query = _dbContext.Registrations
                .Where(r => r.SemesterId == semesterId)
                .Include(r => r.Course)
                .Include(r => r.Student)
                    .ThenInclude(s => s.User)
                .Include(r => r.StudyYear)
                .AsNoTracking()
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
            var query = _dbContext.Registrations
                .Where(r => r.StudyYearId == studyYearId)
                .Include(r => r.Course)
                .Include(r => r.Semester)
                .Include(r => r.Student)
                    .ThenInclude(s => s.User)
                .AsNoTracking()
                .AsQueryable();

            query = ApplyStudyYearFilters(query, filter);

            return await ApplyPaginationAsync(query, filter, cancellationToken);
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET ALL WITH FILTERS
        // ────────────────────────────────────────────────────────────────────────

        public async Task<(IEnumerable<Registration> Data, int TotalCount)> GetAllFilteredAsync(RegistrationFilterQueries? filter = null, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Registrations
                .Include(r => r.Course)
                .Include(r => r.StudyYear)
                .Include(r => r.Semester)
                .Include(r => r.Student)
                    .ThenInclude(s => s.User)
                .AsNoTracking()
                .AsQueryable();

            query = ApplyAllFilters(query, filter);

            return await ApplyPaginationAsync(query, filter, cancellationToken);
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET WITH PAGINATION
        // ────────────────────────────────────────────────────────────────────────

        public async Task<(IEnumerable<Registration> Data, int TotalCount)> GetPaginatedAsync(
            RegistrationFilterQueries? filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Registrations
                .Include(r => r.Course)
                .Include(r => r.StudyYear)
                .Include(r => r.Semester)
                .Include(r => r.Student)
                    .ThenInclude(s => s.User)
                .AsNoTracking()
                .AsQueryable();

            query = ApplyAllFilters(query, filter);

            return await ApplyPaginationAsync(query, filter, cancellationToken);
        }

        // ────────────────────────────────────────────────────────────────────────
        // STUDENT PROGRESS
        // ────────────────────────────────────────────────────────────────────────

        public async Task<IEnumerable<Registration>> GetStudentPassedCoursesAsync(string studentId)
        {
            return await _dbContext.Registrations
                .Where(r => r.StudentId == studentId && r.IsPassed)
                .Include(r => r.Course)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Registration>> GetStudentInProgressCoursesAsync(string studentId)
        {
            return await _dbContext.Registrations
                .Where(r => r.StudentId == studentId && r.Progress == CourseProgress.InProgress)
                .Include(r => r.Course)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Registration>> GetStudentFailedCoursesAsync(string studentId)
        {
            return await _dbContext.Registrations
                .Where(r => r.StudentId == studentId && !r.IsPassed && r.Progress == CourseProgress.Completed)
                .Include(r => r.Course)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Registration>> GetStudentSemesterCoursesAsync(
            string studentId, int studyYearId, int semesterId)
        {
            return await _dbContext.Registrations
                .Where(r => r.StudentId == studentId
                         && r.StudyYearId == studyYearId
                         && r.SemesterId == semesterId)
                .Include(r => r.Course)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Registration>> GetStudentCoursesByStudyYearAsync(string studentId, int studyYearId)
        {
            return await _dbContext.Registrations
                .Where(r => r.StudentId == studentId && r.StudyYearId == studyYearId)
                .Include(r => r.Course)
                .Include(r => r.Semester)
                .AsNoTracking()
                .ToListAsync();
        }

        // ────────────────────────────────────────────────────────────────────────
        // CHECK EXISTENCE
        // ────────────────────────────────────────────────────────────────────────

        public async Task<bool> IsStudentRegisteredInCourseAsync(string studentId, int courseId)
        {
            return await _dbContext.Registrations
                .AnyAsync(r => r.StudentId == studentId && r.CourseId == courseId);
        }

        public async Task<bool> IsCourseCompletedByStudentAsync(string studentId, int courseId)
        {
            return await _dbContext.Registrations
                .AnyAsync(r => r.StudentId == studentId && r.CourseId == courseId && r.IsPassed && r.Progress == CourseProgress.Completed);
        }

        public async Task<bool> IsStudentRegisteredInSemesterAsync(string studentId, int semesterId)
        {
            return await _dbContext.Registrations
                .AnyAsync(r => r.StudentId == studentId && r.SemesterId == semesterId);
        }

        public async Task<bool> IsStudentRegisteredInStudyYearAsync(string studentId, int studyYearId)
        {
            return await _dbContext.Registrations
                .AnyAsync(r => r.StudentId == studentId && r.StudyYearId == studyYearId);
        }

        // ────────────────────────────────────────────────────────────────────────
        // COUNTS
        // ────────────────────────────────────────────────────────────────────────

        public async Task<int> GetRegistrationCountBySemesterAsync(int semesterId)
        {
            return await _dbContext.Registrations
                .CountAsync(r => r.SemesterId == semesterId);
        }

        public async Task<int> GetRegistrationCountByCourseAsync(int courseId)
        {
            return await _dbContext.Registrations
                .CountAsync(r => r.CourseId == courseId);
        }

        public async Task<int> GetRegistrationCountByStudentAsync(string studentId)
        {
            return await _dbContext.Registrations
                .CountAsync(r => r.StudentId == studentId);
        }

        public async Task<int> GetRegistrationCountByStudyYearAsync(int studyYearId)
        {
            return await _dbContext.Registrations
                .CountAsync(r => r.StudyYearId == studyYearId);
        }

        public async Task<int> GetStudentCreditHoursAsync(string studentId, int semesterId)
        {
            return await _dbContext.Registrations
                .Where(r => r.StudentId == studentId && r.SemesterId == semesterId)
                .SumAsync(r => r.Course.Credits);
        }

        public async Task<int> GetStudentTotalCreditHoursAsync(string studentId)
        {
            return await _dbContext.Registrations
                .Where(r => r.StudentId == studentId && r.IsPassed)
                .SumAsync(r => r.Course.Credits);
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
        // PRIVATE FILTER METHODS
        // ────────────────────────────────────────────────────────────────────────

        private IQueryable<Registration> ApplyStudentFilters(
            IQueryable<Registration> query,
            RegistrationFilterQueries? filter)
        {
            if (filter == null) return query;

            // Student Info
            if (!string.IsNullOrEmpty(filter.StudentName))
                query = query.Where(r => r.Student.User.Name.Contains(filter.StudentName));

            if (!string.IsNullOrEmpty(filter.AcademicCode))
                query = query.Where(r => r.Student.AcademicCode.Contains(filter.AcademicCode));

            // Course Info
            if (!string.IsNullOrEmpty(filter.CourseName))
                query = query.Where(r => r.Course.Name.Contains(filter.CourseName));

            if (!string.IsNullOrEmpty(filter.CourseCode))
                query = query.Where(r => r.Course.Code.Contains(filter.CourseCode));

            // Status
            if (filter.Status.HasValue)
                query = query.Where(r => r.Status == filter.Status.Value);

            if (filter.IsPassed.HasValue)
                query = query.Where(r => r.IsPassed == filter.IsPassed.Value);

            if (filter.Progress.HasValue)
                query = query.Where(r => r.Progress == filter.Progress.Value);

            if (filter.Grade.HasValue)
                query = query.Where(r => r.Grade == filter.Grade.Value);

            // Date
            if (filter.RegisteredFrom.HasValue)
                query = query.Where(r => r.RegisteredAt >= filter.RegisteredFrom.Value);

            if (filter.RegisteredTo.HasValue)
                query = query.Where(r => r.RegisteredAt <= filter.RegisteredTo.Value);

            // Search
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

            // Student Info
            if (!string.IsNullOrEmpty(filter.StudentName))
                query = query.Where(r => r.Student.User.Name.Contains(filter.StudentName));

            if (!string.IsNullOrEmpty(filter.AcademicCode))
                query = query.Where(r => r.Student.AcademicCode.Contains(filter.AcademicCode));

            // Course Info
            if (!string.IsNullOrEmpty(filter.CourseName))
                query = query.Where(r => r.Course.Name.Contains(filter.CourseName));

            if (!string.IsNullOrEmpty(filter.CourseCode))
                query = query.Where(r => r.Course.Code.Contains(filter.CourseCode));

            // Status
            if (filter.Status.HasValue)
                query = query.Where(r => r.Status == filter.Status.Value);

            if (filter.IsPassed.HasValue)
                query = query.Where(r => r.IsPassed == filter.IsPassed.Value);

            if (filter.Progress.HasValue)
                query = query.Where(r => r.Progress == filter.Progress.Value);

            if (filter.Grade.HasValue)
                query = query.Where(r => r.Grade == filter.Grade.Value);

            // Date
            if (filter.RegisteredFrom.HasValue)
                query = query.Where(r => r.RegisteredAt >= filter.RegisteredFrom.Value);

            if (filter.RegisteredTo.HasValue)
                query = query.Where(r => r.RegisteredAt <= filter.RegisteredTo.Value);

            // Search
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

            // Student Info
            if (!string.IsNullOrEmpty(filter.StudentName))
                query = query.Where(r => r.Student.User.Name.Contains(filter.StudentName));

            if (!string.IsNullOrEmpty(filter.AcademicCode))
                query = query.Where(r => r.Student.AcademicCode.Contains(filter.AcademicCode));

            // Course Info
            if (!string.IsNullOrEmpty(filter.CourseName))
                query = query.Where(r => r.Course.Name.Contains(filter.CourseName));

            if (!string.IsNullOrEmpty(filter.CourseCode))
                query = query.Where(r => r.Course.Code.Contains(filter.CourseCode));

            // Semester
            if (filter.SemesterTitle.HasValue)
                query = query.Where(r => r.Semester.Title == filter.SemesterTitle.Value);

            // Status
            if (filter.Status.HasValue)
                query = query.Where(r => r.Status == filter.Status.Value);

            if (filter.IsPassed.HasValue)
                query = query.Where(r => r.IsPassed == filter.IsPassed.Value);

            if (filter.Progress.HasValue)
                query = query.Where(r => r.Progress == filter.Progress.Value);

            if (filter.Grade.HasValue)
                query = query.Where(r => r.Grade == filter.Grade.Value);

            // Date
            if (filter.RegisteredFrom.HasValue)
                query = query.Where(r => r.RegisteredAt >= filter.RegisteredFrom.Value);

            if (filter.RegisteredTo.HasValue)
                query = query.Where(r => r.RegisteredAt <= filter.RegisteredTo.Value);

            // Search
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

        private IQueryable<Registration> ApplyAllFilters(
            IQueryable<Registration> query,
            RegistrationFilterQueries? filter)
        {
            if (filter == null) return query;

            // Student Info
            if (!string.IsNullOrEmpty(filter.StudentName))
                query = query.Where(r => r.Student.User.Name.Contains(filter.StudentName));

            if (!string.IsNullOrEmpty(filter.AcademicCode))
                query = query.Where(r => r.Student.AcademicCode.Contains(filter.AcademicCode));

            // Course Info
            if (!string.IsNullOrEmpty(filter.CourseName))
                query = query.Where(r => r.Course.Name.Contains(filter.CourseName));

            if (!string.IsNullOrEmpty(filter.CourseCode))
                query = query.Where(r => r.Course.Code.Contains(filter.CourseCode));

            // Status
            if (filter.Status.HasValue)
                query = query.Where(r => r.Status == filter.Status.Value);

            if (filter.IsPassed.HasValue)
                query = query.Where(r => r.IsPassed == filter.IsPassed.Value);

            if (filter.Progress.HasValue)
                query = query.Where(r => r.Progress == filter.Progress.Value);

            if (filter.Grade.HasValue)
                query = query.Where(r => r.Grade == filter.Grade.Value);

            // Study Year Range
            if (filter.StudyYearStart.HasValue)
                query = query.Where(r => r.StudyYear.StartYear >= filter.StudyYearStart.Value);

            if (filter.StudyYearEnd.HasValue)
                query = query.Where(r => r.StudyYear.EndYear <= filter.StudyYearEnd.Value);

            // Semester
            if (filter.SemesterTitle.HasValue)
                query = query.Where(r => r.Semester.Title == filter.SemesterTitle.Value);

            // Date
            if (filter.RegisteredFrom.HasValue)
                query = query.Where(r => r.RegisteredAt >= filter.RegisteredFrom.Value);

            if (filter.RegisteredTo.HasValue)
                query = query.Where(r => r.RegisteredAt <= filter.RegisteredTo.Value);

            // Search
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

            // Default pagination
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
                "studyyear" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(r => r.StudyYear.StartYear)
                    : query.OrderByDescending(r => r.StudyYear.StartYear),
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