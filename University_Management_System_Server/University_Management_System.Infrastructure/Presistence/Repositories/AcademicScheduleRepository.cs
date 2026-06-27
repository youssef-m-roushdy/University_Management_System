using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Queries.AcademicScheduleQueries;
using Microsoft.EntityFrameworkCore;
using University_Management_System.Domain.Queries;

namespace University_Management_System.Infrastructure.Presistence.Repositories
{
    public class AcademicScheduleRepository : GenericRepository<AcademicSchedule, int>, IAcademicScheduleRepository
    {
        public AcademicScheduleRepository(UniversityDbContext dbContext) : base(dbContext)
        {
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY DEPARTMENT AND SEMESTER (Student, Instructor, Assistant)
        // ────────────────────────────────────────────────────────────────────────

        public async Task<(IEnumerable<AcademicSchedule> Data, int TotalCount)> GetByDepartmentAndSemesterAsync(
            int departmentId,
            int semesterId,
            AcademicScheduleDepartmentSemesterQueries? filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = GetQueryable()
                .Include(a => a.Department)
                .Include(a => a.Semester)
                .Include(a => a.StudyYear)
                .Include(a => a.Admin)
                .Where(a => a.DepartmentId == departmentId && a.SemesterId == semesterId)
                .AsQueryable();

            // ─── Apply filters ──────────────────────────────────────────────────
            if (filter != null)
            {
                if (filter.ScheduleDateFrom.HasValue)
                    query = query.Where(a => a.ScheduleDate >= filter.ScheduleDateFrom.Value);

                if (filter.ScheduleDateTo.HasValue)
                    query = query.Where(a => a.ScheduleDate <= filter.ScheduleDateTo.Value);

                if (!string.IsNullOrEmpty(filter.SearchTerm))
                {
                    var searchTerm = filter.SearchTerm.ToLower();
                    query = query.Where(a =>
                        a.Title.ToLower().Contains(searchTerm) ||
                        (a.Description != null && a.Description.ToLower().Contains(searchTerm)) ||
                        a.Department.Name.ToLower().Contains(searchTerm));
                }
            }

            var totalCount = await query.CountAsync(cancellationToken);

            // ─── Apply sorting ──────────────────────────────────────────────────
            query = filter?.SortBy?.ToLower() switch
            {
                "title" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(a => a.Title)
                    : query.OrderByDescending(a => a.Title),
                "scheduledate" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(a => a.ScheduleDate)
                    : query.OrderByDescending(a => a.ScheduleDate),
                "department" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(a => a.Department.Name)
                    : query.OrderByDescending(a => a.Department.Name),
                _ => query.OrderBy(a => a.ScheduleDate)
            };

            // ─── Apply pagination ──────────────────────────────────────────────
            var pageNumber = filter?.PageNumber ?? 1;
            var pageSize = filter?.PageSize ?? 10;

            var result = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (result, totalCount);
        }

        // ────────────────────────────────────────────────────────────────────────
        // ✅ GET BY DEPARTMENT AND SEMESTER WITH DETAILS
        // ────────────────────────────────────────────────────────────────────────

        public async Task<IEnumerable<AcademicSchedule>> GetByDepartmentAndSemesterWithDetailsAsync(
            int departmentId,
            int semesterId)
        {
            return await GetQueryable()
                .Include(a => a.Department)
                .Include(a => a.Semester)
                    .ThenInclude(s => s.StudyYear)
                .Include(a => a.StudyYear)
                .Include(a => a.Admin)
                .Where(a => a.DepartmentId == departmentId && a.SemesterId == semesterId)
                .OrderBy(a => a.ScheduleDate)
                .ToListAsync();
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY DEPARTMENT (Admin only)
        // ────────────────────────────────────────────────────────────────────────

        public async Task<(IEnumerable<AcademicSchedule> Data, int TotalCount)> GetByDepartmentIdAsync(
            int departmentId,
            AcademicScheduleSemesterQueries? filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = GetQueryable()
                .Include(a => a.Department)
                .Include(a => a.Semester)
                .Include(a => a.StudyYear)
                .Include(a => a.Admin)
                .Where(a => a.DepartmentId == departmentId)
                .AsQueryable();

            // ─── Apply filters ──────────────────────────────────────────────────
            if (filter != null)
            {
                if (filter.ScheduleDateFrom.HasValue)
                    query = query.Where(a => a.ScheduleDate >= filter.ScheduleDateFrom.Value);

                if (filter.ScheduleDateTo.HasValue)
                    query = query.Where(a => a.ScheduleDate <= filter.ScheduleDateTo.Value);

                if (!string.IsNullOrEmpty(filter.SearchTerm))
                {
                    var searchTerm = filter.SearchTerm.ToLower();
                    query = query.Where(a =>
                        a.Title.ToLower().Contains(searchTerm) ||
                        (a.Description != null && a.Description.ToLower().Contains(searchTerm)) ||
                        a.Department.Name.ToLower().Contains(searchTerm));
                }
            }

            var totalCount = await query.CountAsync(cancellationToken);

            // ─── Apply sorting ──────────────────────────────────────────────────
            query = filter?.SortBy?.ToLower() switch
            {
                "title" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(a => a.Title)
                    : query.OrderByDescending(a => a.Title),
                "scheduledate" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(a => a.ScheduleDate)
                    : query.OrderByDescending(a => a.ScheduleDate),
                "semester" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(a => a.Semester.Title)
                    : query.OrderByDescending(a => a.Semester.Title),
                "department" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(a => a.Department.Name)
                    : query.OrderByDescending(a => a.Department.Name),
                _ => query.OrderByDescending(a => a.ScheduleDate)
            };

            // ─── Apply pagination ──────────────────────────────────────────────
            var pageNumber = filter?.PageNumber ?? 1;
            var pageSize = filter?.PageSize ?? 10;

            var result = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (result, totalCount);
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY SEMESTER
        // ────────────────────────────────────────────────────────────────────────

        public async Task<IEnumerable<AcademicSchedule>> GetBySemesterIdAsync(int semesterId)
        {
            return await GetQueryable()
                .Include(a => a.Department)
                .Where(a => a.SemesterId == semesterId)
                .OrderBy(a => a.ScheduleDate)
                .ToListAsync();
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY STUDY YEAR
        // ────────────────────────────────────────────────────────────────────────

        public async Task<IEnumerable<AcademicSchedule>> GetByStudyYearIdAsync(int studyYearId)
        {
            return await GetQueryable()
                .Include(a => a.Department)
                .Include(a => a.Semester)
                .Where(a => a.StudyYearId == studyYearId)
                .OrderBy(a => a.ScheduleDate)
                .ToListAsync();
        }

        // ────────────────────────────────────────────────────────────────────────
        // CHECK EXISTENCE
        // ────────────────────────────────────────────────────────────────────────

        public async Task<bool> ExistsAsync(int departmentId, int semesterId, string title)
        {
            return await GetQueryable()
                .AnyAsync(a => a.DepartmentId == departmentId
                               && a.SemesterId == semesterId
                               && a.Title == title);
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await GetQueryable()
                .AnyAsync(a => a.Id == id);
        }
        public async Task<AcademicSchedule?> GetByIdWithDetailsAsync(int id)
        {
            return await GetQueryable()
                .Include(a => a.Department)
                .Include(a => a.Semester)
                    .ThenInclude(s => s.StudyYear)
                .Include(a => a.StudyYear)
                .Include(a => a.Admin)
                    .ThenInclude(ad => ad.User)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}