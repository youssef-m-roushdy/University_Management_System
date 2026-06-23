using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;
using University_Management_System.Domain.Queries;
using University_Management_System.Domain.Queries.AcademicScheduleQueries;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Infrastructure.Presistence.Repositories
{
    public class AcademicScheduleRepository : GenericRepository<AcademicSchedule, int>, IAcademicScheduleRepository
    {
        public AcademicScheduleRepository(UniversityDbContext dbContext) : base(dbContext)
        {
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY STUDY YEAR
        // ────────────────────────────────────────────────────────────────────────

        public async Task<(IEnumerable<AcademicSchedule> Data, int TotalCount)> GetByStudyYearIdAsync(
            int studyYearId,
            AcademicScheduleStudyYearQueries? filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = BuildBaseQuery()
                .Where(s => s.StudyYearId == studyYearId);

            query = ApplyStudyYearFilters(query, filter);

            return await ApplyPaginationAsync(query, filter, cancellationToken);
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY SEMESTER
        // ────────────────────────────────────────────────────────────────────────

        public async Task<(IEnumerable<AcademicSchedule> Data, int TotalCount)> GetBySemesterIdAsync(
            int semesterId,
            AcademicScheduleSemesterQueries? filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = BuildBaseQuery()
                .Where(s => s.SemesterId == semesterId);

            query = ApplySemesterFilters(query, filter);

            return await ApplyPaginationAsync(query, filter, cancellationToken);
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY DEPARTMENT
        // ────────────────────────────────────────────────────────────────────────

        public async Task<(IEnumerable<AcademicSchedule> Data, int TotalCount)> GetByDepartmentIdAsync(
            int departmentId,
            AcademicScheduleDepartmentQueries? filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = BuildBaseQuery()
                .Where(s => s.DepartmentId == departmentId);

            query = ApplyDepartmentFilters(query, filter);

            return await ApplyPaginationAsync(query, filter, cancellationToken);
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY DEPARTMENT AND SEMESTER
        // ────────────────────────────────────────────────────────────────────────

        public async Task<(IEnumerable<AcademicSchedule> Data, int TotalCount)> GetByDepartmentAndSemesterAsync(
            int departmentId,
            int semesterId,
            AcademicScheduleDepartmentSemesterQueries? filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = BuildBaseQuery()
                .Where(s => s.DepartmentId == departmentId && s.SemesterId == semesterId);

            query = ApplyDepartmentSemesterFilters(query, filter);

            return await ApplyPaginationAsync(query, filter, cancellationToken);
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET ALL WITH PAGINATION
        // ────────────────────────────────────────────────────────────────────────

        public async Task<(IEnumerable<AcademicSchedule> Data, int TotalCount)> GetAllAsync(
            AcademicScheduleFilterQueries? filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = BuildBaseQuery();

            query = ApplyFilters(query, filter);

            return await ApplyPaginationAsync(query, filter, cancellationToken);
        }

        // ────────────────────────────────────────────────────────────────────────
        // CHECK EXISTENCE
        // ────────────────────────────────────────────────────────────────────────

        public async Task<bool> ScheduleExistsAsync(int studyYearId, int semesterId, int departmentId)
        {
            return await _dbContext.AcademicSchedules
                .AnyAsync(s => s.StudyYearId == studyYearId 
                            && s.SemesterId == semesterId 
                            && s.DepartmentId == departmentId);
        }

        public async Task<bool> ScheduleExistsByIdAsync(int scheduleId)
        {
            return await _dbContext.AcademicSchedules
                .AnyAsync(s => s.Id == scheduleId);
        }

        // ────────────────────────────────────────────────────────────────────────
        // COUNTS
        // ────────────────────────────────────────────────────────────────────────

        public async Task<int> GetCountByStudyYearAsync(int studyYearId)
        {
            return await _dbContext.AcademicSchedules
                .CountAsync(s => s.StudyYearId == studyYearId);
        }

        public async Task<int> GetCountBySemesterAsync(int semesterId)
        {
            return await _dbContext.AcademicSchedules
                .CountAsync(s => s.SemesterId == semesterId);
        }

        public async Task<int> GetCountByDepartmentAsync(int departmentId)
        {
            return await _dbContext.AcademicSchedules
                .CountAsync(s => s.DepartmentId == departmentId);
        }

        // ────────────────────────────────────────────────────────────────────────
        // BULK OPERATIONS
        // ────────────────────────────────────────────────────────────────────────

        public async Task AddRangeAsync(IEnumerable<AcademicSchedule> schedules)
        {
            await _dbContext.AcademicSchedules.AddRangeAsync(schedules);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(IEnumerable<AcademicSchedule> schedules)
        {
            _dbContext.AcademicSchedules.UpdateRange(schedules);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<AcademicSchedule> schedules)
        {
            _dbContext.AcademicSchedules.RemoveRange(schedules);
            await _dbContext.SaveChangesAsync();
        }

        // ────────────────────────────────────────────────────────────────────────
        // PRIVATE HELPERS
        // ────────────────────────────────────────────────────────────────────────

        private IQueryable<AcademicSchedule> BuildBaseQuery()
        {
            return _dbContext.AcademicSchedules
                .Include(s => s.Department)
                .Include(s => s.Semester)
                .Include(s => s.StudyYear)
                .Include(s => s.Admin)
                .AsNoTracking()
                .AsQueryable();
        }

        private IQueryable<AcademicSchedule> ApplyFilters(
            IQueryable<AcademicSchedule> query, 
            AcademicScheduleFilterQueries? filter)
        {
            if (filter == null) return query;

            // ─── Department Filters ──────────────────────────────────────────
            if (!string.IsNullOrEmpty(filter.DepartmentName))
                query = query.Where(s => s.Department.Name.Contains(filter.DepartmentName));

            if (!string.IsNullOrEmpty(filter.DepartmentCode))
                query = query.Where(s => s.Department.Code.Contains(filter.DepartmentCode));

            // ─── Schedule Info Filters ─────────────────────────────────────
            if (!string.IsNullOrEmpty(filter.Title))
                query = query.Where(s => s.Title.Contains(filter.Title));

            if (filter.ScheduleDateFrom.HasValue)
                query = query.Where(s => s.ScheduleDate >= filter.ScheduleDateFrom.Value);

            if (filter.ScheduleDateTo.HasValue)
                query = query.Where(s => s.ScheduleDate <= filter.ScheduleDateTo.Value);

            // ─── Study Year Range Filters ──────────────────────────────────
            if (filter.StudyYearStart.HasValue)
                query = query.Where(s => s.StudyYear.StartYear >= filter.StudyYearStart.Value);

            if (filter.StudyYearEnd.HasValue)
                query = query.Where(s => s.StudyYear.EndYear <= filter.StudyYearEnd.Value);
            
            // ─── Semester Filters ──────────────────────────────────────────
            if (filter.SemesterTitle.HasValue)
                query = query.Where(s => s.Semester.Title == filter.SemesterTitle.Value);

            // ─── Search Term ──────────────────────────────────────────────
            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                var searchTerm = filter.SearchTerm.ToLower();
                query = query.Where(s =>
                    s.Title.ToLower().Contains(searchTerm) ||
                    s.Description != null && s.Description.ToLower().Contains(searchTerm) ||
                    s.Department.Name.ToLower().Contains(searchTerm) ||
                    s.Department.Code.ToLower().Contains(searchTerm));
            }

            return query;
        }

        private IQueryable<AcademicSchedule> ApplyStudyYearFilters(
            IQueryable<AcademicSchedule> query, 
            AcademicScheduleStudyYearQueries? filter)
        {
            if (filter == null) return query;

            // ─── Department Filters ──────────────────────────────────────────
            if (!string.IsNullOrEmpty(filter.DepartmentName))
                query = query.Where(s => s.Department.Name.Contains(filter.DepartmentName));

            if (!string.IsNullOrEmpty(filter.DepartmentCode))
                query = query.Where(s => s.Department.Code.Contains(filter.DepartmentCode));

            // ─── Schedule Info Filters ─────────────────────────────────────
            if (!string.IsNullOrEmpty(filter.Title))
                query = query.Where(s => s.Title.Contains(filter.Title));

            if (filter.ScheduleDateFrom.HasValue)
                query = query.Where(s => s.ScheduleDate >= filter.ScheduleDateFrom.Value);

            if (filter.ScheduleDateTo.HasValue)
                query = query.Where(s => s.ScheduleDate <= filter.ScheduleDateTo.Value);

            // ─── Semester Filters ──────────────────────────────────────────
            if (filter.SemesterTitle.HasValue)
                query = query.Where(s => s.Semester.Title == filter.SemesterTitle.Value);

            // ─── Search Term ──────────────────────────────────────────────
            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                var searchTerm = filter.SearchTerm.ToLower();
                query = query.Where(s =>
                    s.Title.ToLower().Contains(searchTerm) ||
                    s.Description != null && s.Description.ToLower().Contains(searchTerm) ||
                    s.Department.Name.ToLower().Contains(searchTerm) ||
                    s.Department.Code.ToLower().Contains(searchTerm));
            }

            return query;
        }

        private IQueryable<AcademicSchedule> ApplySemesterFilters(
            IQueryable<AcademicSchedule> query, 
            AcademicScheduleSemesterQueries? filter)
        {
            if (filter == null) return query;

            // ─── Department Filters ──────────────────────────────────────────
            if (!string.IsNullOrEmpty(filter.DepartmentName))
                query = query.Where(s => s.Department.Name.Contains(filter.DepartmentName));

            if (!string.IsNullOrEmpty(filter.DepartmentCode))
                query = query.Where(s => s.Department.Code.Contains(filter.DepartmentCode));

            // ─── Schedule Info Filters ─────────────────────────────────────
            if (!string.IsNullOrEmpty(filter.Title))
                query = query.Where(s => s.Title.Contains(filter.Title));

            if (filter.ScheduleDateFrom.HasValue)
                query = query.Where(s => s.ScheduleDate >= filter.ScheduleDateFrom.Value);

            if (filter.ScheduleDateTo.HasValue)
                query = query.Where(s => s.ScheduleDate <= filter.ScheduleDateTo.Value);

            // ─── Search Term ──────────────────────────────────────────────
            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                var searchTerm = filter.SearchTerm.ToLower();
                query = query.Where(s =>
                    s.Title.ToLower().Contains(searchTerm) ||
                    s.Description != null && s.Description.ToLower().Contains(searchTerm) ||
                    s.Department.Name.ToLower().Contains(searchTerm) ||
                    s.Department.Code.ToLower().Contains(searchTerm));
            }

            return query;
        }

        private IQueryable<AcademicSchedule> ApplyDepartmentFilters(
            IQueryable<AcademicSchedule> query, 
            AcademicScheduleDepartmentQueries? filter)
        {
            if (filter == null) return query;

            // ─── Schedule Info Filters ─────────────────────────────────────
            if (!string.IsNullOrEmpty(filter.Title))
                query = query.Where(s => s.Title.Contains(filter.Title));

            if (filter.ScheduleDateFrom.HasValue)
                query = query.Where(s => s.ScheduleDate >= filter.ScheduleDateFrom.Value);

            if (filter.ScheduleDateTo.HasValue)
                query = query.Where(s => s.ScheduleDate <= filter.ScheduleDateTo.Value);

            // ─── Study Year Range Filters ──────────────────────────────────
            if (filter.StudyYearStart.HasValue)
                query = query.Where(s => s.StudyYear.StartYear >= filter.StudyYearStart.Value);

            if (filter.StudyYearEnd.HasValue)
                query = query.Where(s => s.StudyYear.EndYear <= filter.StudyYearEnd.Value);

            // ─── Semester Filters ──────────────────────────────────────────
            if (filter.SemesterTitle.HasValue)
                query = query.Where(s => s.Semester.Title == filter.SemesterTitle.Value);

            // ─── Search Term ──────────────────────────────────────────────
            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                var searchTerm = filter.SearchTerm.ToLower();
                query = query.Where(s =>
                    s.Title.ToLower().Contains(searchTerm) ||
                    s.Description != null && s.Description.ToLower().Contains(searchTerm) ||
                    s.Department.Name.ToLower().Contains(searchTerm) ||
                    s.Department.Code.ToLower().Contains(searchTerm));
            }

            return query;
        }

        private IQueryable<AcademicSchedule> ApplyDepartmentSemesterFilters(
            IQueryable<AcademicSchedule> query, 
            AcademicScheduleDepartmentSemesterQueries? filter)
        {
            if (filter == null) return query;

            // ─── Schedule Info Filters ─────────────────────────────────────
            if (!string.IsNullOrEmpty(filter.Title))
                query = query.Where(s => s.Title.Contains(filter.Title));

            if (filter.ScheduleDateFrom.HasValue)
                query = query.Where(s => s.ScheduleDate >= filter.ScheduleDateFrom.Value);

            if (filter.ScheduleDateTo.HasValue)
                query = query.Where(s => s.ScheduleDate <= filter.ScheduleDateTo.Value);

            // ─── Study Year Range Filters ──────────────────────────────────
            if (filter.StudyYearStart.HasValue)
                query = query.Where(s => s.StudyYear.StartYear >= filter.StudyYearStart.Value);

            if (filter.StudyYearEnd.HasValue)
                query = query.Where(s => s.StudyYear.EndYear <= filter.StudyYearEnd.Value);

            // ─── Search Term ──────────────────────────────────────────────
            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                var searchTerm = filter.SearchTerm.ToLower();
                query = query.Where(s =>
                    s.Title.ToLower().Contains(searchTerm) ||
                    s.Description != null && s.Description.ToLower().Contains(searchTerm) ||
                    s.Department.Name.ToLower().Contains(searchTerm) ||
                    s.Department.Code.ToLower().Contains(searchTerm));
            }

            return query;
        }

        private async Task<(IEnumerable<AcademicSchedule> Data, int TotalCount)> ApplyPaginationAsync(
            IQueryable<AcademicSchedule> query,
            SearchablePaginationQuery? filter,
            CancellationToken cancellationToken)
        {
            var totalCount = await query.CountAsync(cancellationToken);

            var pageNumber = filter?.PageNumber ?? 1;
            var pageSize = filter?.PageSize ?? 10;

            // Apply sorting
            query = filter?.SortBy?.ToLower() switch
            {
                "title" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(s => s.Title)
                    : query.OrderByDescending(s => s.Title),
                "department" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(s => s.Department.Name)
                    : query.OrderByDescending(s => s.Department.Name),
                "scheduleDate" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(s => s.ScheduleDate)
                    : query.OrderByDescending(s => s.ScheduleDate),
                "studyyear" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(s => s.StudyYear.StartYear)
                    : query.OrderByDescending(s => s.StudyYear.StartYear),
                "semester" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(s => s.Semester.Title)
                    : query.OrderByDescending(s => s.Semester.Title),
                _ => query.OrderBy(s => s.ScheduleDate)
            };

            var result = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (result, totalCount);
        }
    }
}