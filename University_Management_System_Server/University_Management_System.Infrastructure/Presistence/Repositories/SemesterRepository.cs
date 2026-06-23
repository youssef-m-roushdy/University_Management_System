using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;
using University_Management_System.Domain.Queries;
using University_Management_System.Domain.Enums;
using University_Management_System.Domain.Queries.SemesterQueries;

namespace University_Management_System.Infrastructure.Presistence.Repositories
{
    public class SemesterRepository : GenericRepository<Semester, int>, ISemesterRepository
    {
        public SemesterRepository(UniversityDbContext dbContext) : base(dbContext)
        {
        }

        // ────────────────────────────────────────────────────────────────────────
        // CHECK EXISTENCE
        // ────────────────────────────────────────────────────────────────────────

        public async Task<bool> IsActiveSemesterAsync(int semesterId)
        {
            return await GetQueryable()
                .AnyAsync(s => s.Id == semesterId && s.IsActive);
        }

        public async Task<bool> IsSemesterBelongsToStudyYearAsync(int semesterId, int studyYearId)
        {
            return await GetQueryable()
                .AnyAsync(s => s.Id == semesterId && s.StudyYearId == studyYearId);
        }

        public async Task<bool> SemesterTitleExistsInStudyYearAsync(int studyYearId, SemesterEnum title)
        {
            return await _dbContext.Semesters
                .AnyAsync(s => s.StudyYearId == studyYearId && s.Title == title);
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET COLLECTIONS
        // ────────────────────────────────────────────────────────────────────────

        public async Task<IEnumerable<Semester>> GetByStudyYearIdAsync(int studyYearId)
        {
            return await GetQueryable()
                .Where(s => s.StudyYearId == studyYearId)
                .OrderBy(s => s.Title)
                .ToListAsync();
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET WITH PAGINATION
        // ────────────────────────────────────────────────────────────────────────

        public async Task<(IEnumerable<Semester> Data, int TotalCount)> GetAllFilteredAsync(
            SemesterFilterQueries query,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Semester> semesters = GetQueryable()
                .Include(s => s.StudyYear);

            if (query.IsActive.HasValue)
                semesters = semesters.Where(s => s.IsActive == query.IsActive.Value);

            if (query.StartDateFrom.HasValue)
                semesters = semesters.Where(s => s.StartDate >= query.StartDateFrom.Value);

            if (query.StartDateTo.HasValue)
                semesters = semesters.Where(s => s.StartDate <= query.StartDateTo.Value);

            if (query.EndDateFrom.HasValue)
                semesters = semesters.Where(s => s.EndDate >= query.EndDateFrom.Value);

            if (query.EndDateTo.HasValue)
                semesters = semesters.Where(s => s.EndDate <= query.EndDateTo.Value);

            var totalCount = await semesters.CountAsync(cancellationToken);

            // Apply sorting
            semesters = query.SortBy?.ToLower() switch
            {
                "title" => query.SortDirection == SortDirection.Ascending
                    ? semesters.OrderBy(s => s.Title)
                    : semesters.OrderByDescending(s => s.Title),
                "startdate" => query.SortDirection == SortDirection.Ascending
                    ? semesters.OrderBy(s => s.StartDate)
                    : semesters.OrderByDescending(s => s.StartDate),
                "enddate" => query.SortDirection == SortDirection.Ascending
                    ? semesters.OrderBy(s => s.EndDate)
                    : semesters.OrderByDescending(s => s.EndDate),
                "isactive" => query.SortDirection == SortDirection.Ascending
                    ? semesters.OrderBy(s => s.IsActive)
                    : semesters.OrderByDescending(s => s.IsActive),
                "studyyear" => query.SortDirection == SortDirection.Ascending
                    ? semesters.OrderBy(s => s.StudyYear.StartYear)
                    : semesters.OrderByDescending(s => s.StudyYear.StartYear),
                _ => semesters.OrderBy(s => s.StartDate)
            };

            var result = await semesters
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);

            return (result, totalCount);
        }

        // ────────────────────────────────────────────────────────────────────────
        // CREATE
        // ────────────────────────────────────────────────────────────────────────

        public async Task<Semester> CreateStudyYearSemesterAsync(int studyYearId, Semester semester)
        {
            semester.StudyYearId = studyYearId;
            await _dbContext.Semesters.AddAsync(semester);
            return semester;
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET WITH DETAILS
        // ────────────────────────────────────────────────────────────────────────

        public async Task<Semester?> GetSemesterWithDetailsAsync(int semesterId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Semesters
                .Include(s => s.StudyYear)
                .Include(s => s.Registrations)
                .Include(s => s.AcademicSchedules)
                .FirstOrDefaultAsync(s => s.Id == semesterId, cancellationToken);
        }

        public async Task<Semester?> GetSemesterWithFullDetailsAsync(int semesterId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Semesters
                .Include(s => s.StudyYear)
                    .ThenInclude(sy => sy.StudentStudyYears)
                .Include(s => s.Registrations)
                    .ThenInclude(r => r.Student)
                        .ThenInclude(st => st.User)
                .Include(s => s.Registrations)
                    .ThenInclude(r => r.Course)
                        .ThenInclude(c => c.Department)
                .Include(s => s.AcademicSchedules)
                .Include(s => s.SemesterGPAs)
                    .ThenInclude(sg => sg.Student)
                .Include(s => s.CourseAssistants)
                    .ThenInclude(ca => ca.Assistant)
                        .ThenInclude(a => a.User)
                .Include(s => s.CourseAssistants)
                    .ThenInclude(ca => ca.Course)
                .Include(s => s.CourseInstructors)
                    .ThenInclude(ci => ci.Instructor)
                        .ThenInclude(i => i.User)
                .Include(s => s.CourseInstructors)
                    .ThenInclude(ci => ci.Course)
                .Include(s => s.InstructorCourseUploads)
                .Include(s => s.AssistantCourseUploads)
                .FirstOrDefaultAsync(s => s.Id == semesterId, cancellationToken);
        }
    }
}