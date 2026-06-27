using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Enums;
using University_Management_System.Domain.Queries.SemesterGPAQueries;
using Microsoft.EntityFrameworkCore;
using University_Management_System.Domain.Queries;

namespace University_Management_System.Infrastructure.Presistence.Repositories
{
    public class SemesterGPARepository : GenericRepository<SemesterGPA, int>, ISemesterGPARepository
    {
        public SemesterGPARepository(UniversityDbContext dbContext) : base(dbContext)
        {
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY STUDENT
        // ────────────────────────────────────────────────────────────────────────

        public async Task<IEnumerable<SemesterGPA>> GetByStudentIdAsync(string studentId)
        {
            return await BuildBaseQuery()
                .Where(g => g.StudentId == studentId)
                .OrderByDescending(g => g.StudyYear.StartYear)
                .ThenBy(g => g.Semester.Title)
                .ToListAsync();
        }

        public async Task<IEnumerable<SemesterGPA>> GetByStudentIdWithDetailsAsync(string studentId)
        {
            return await BuildBaseQuery()
                .Where(g => g.StudentId == studentId)
                .OrderByDescending(g => g.StudyYear.StartYear)
                .ThenBy(g => g.Semester.Title)
                .ToListAsync();
        }

        public async Task<(IEnumerable<SemesterGPA> Data, int TotalCount)> GetByStudentIdPaginatedAsync(
            string studentId,
            semesterGPAStudyYearQueries? filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = BuildBaseQuery()
                .Where(g => g.StudentId == studentId)
                .AsQueryable();

            query = ApplyStudyYearFilters(query, filter);

            var totalCount = await query.CountAsync(cancellationToken);

            query = ApplySorting(query, filter);

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

        public async Task<IEnumerable<SemesterGPA>> GetBySemesterIdAsync(int semesterId)
        {
            return await BuildBaseQuery()
                .Where(g => g.SemesterId == semesterId)
                .OrderByDescending(g => g.GPA)
                .ToListAsync();
        }

        public async Task<(IEnumerable<SemesterGPA> Data, int TotalCount)> GetBySemesterIdPaginatedAsync(
            int semesterId,
            SemesterGPAFilterInSemesterQueries? filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = BuildBaseQuery()
                .Where(g => g.SemesterId == semesterId)
                .AsQueryable();

            query = ApplySemesterFilters(query, filter);

            var totalCount = await query.CountAsync(cancellationToken);

            query = ApplySorting(query, filter);

            var pageNumber = filter?.PageNumber ?? 1;
            var pageSize = filter?.PageSize ?? 10;

            var result = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (result, totalCount);
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY STUDY YEAR
        // ────────────────────────────────────────────────────────────────────────

        public async Task<IEnumerable<SemesterGPA>> GetByStudyYearIdAsync(int studyYearId)
        {
            return await BuildBaseQuery()
                .Where(g => g.StudyYearId == studyYearId)
                .OrderByDescending(g => g.GPA)
                .ToListAsync();
        }

        public async Task<(IEnumerable<SemesterGPA> Data, int TotalCount)> GetByStudyYearIdPaginatedAsync(
            int studyYearId,
            semesterGPAStudyYearQueries? filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = BuildBaseQuery()
                .Where(g => g.StudyYearId == studyYearId)
                .AsQueryable();

            query = ApplyStudyYearFilters(query, filter);

            var totalCount = await query.CountAsync(cancellationToken);

            query = ApplySorting(query, filter);

            var pageNumber = filter?.PageNumber ?? 1;
            var pageSize = filter?.PageSize ?? 10;

            var result = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (result, totalCount);
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY STUDENT AND SEMESTER
        // ────────────────────────────────────────────────────────────────────────

        public async Task<SemesterGPA?> GetByStudentAndSemesterAsync(string studentId, int semesterId)
        {
            return await BuildBaseQuery()
                .FirstOrDefaultAsync(g => g.StudentId == studentId && g.SemesterId == semesterId);
        }

        public async Task<SemesterGPA?> GetByStudentAndSemesterWithDetailsAsync(string studentId, int semesterId)
        {
            return await BuildBaseQuery()
                .FirstOrDefaultAsync(g => g.StudentId == studentId && g.SemesterId == semesterId);
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY STUDENT AND STUDY YEAR
        // ────────────────────────────────────────────────────────────────────────

        public async Task<IEnumerable<SemesterGPA>> GetByStudentAndStudyYearAsync(string studentId, int studyYearId)
        {
            return await BuildBaseQuery()
                .Where(g => g.StudentId == studentId && g.StudyYearId == studyYearId)
                .OrderBy(g => g.Semester.Title)
                .ToListAsync();
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET ALL WITH FILTERS - UPDATED
        // ────────────────────────────────────────────────────────────────────────

        public async Task<(IEnumerable<SemesterGPA> Data, int TotalCount)> GetAllFilteredAsync(
            SemesterGPAFilterQueries? filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = BuildBaseQuery()
                .AsQueryable();

            // Apply all filters from SemesterGPAFilterQueries
            query = ApplyAllFilters(query, filter);

            var totalCount = await query.CountAsync(cancellationToken);

            query = ApplySortingForFilterQueries(query, filter);

            var pageNumber = filter?.PageNumber ?? 1;
            var pageSize = filter?.PageSize ?? 10;

            var result = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (result, totalCount);
        }

        // ────────────────────────────────────────────────────────────────────────
        // CHECK EXISTENCE
        // ────────────────────────────────────────────────────────────────────────

        public async Task<bool> ExistsAsync(string studentId, int semesterId)
        {
            return await GetQueryable()
                .AnyAsync(g => g.StudentId == studentId && g.SemesterId == semesterId);
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await GetQueryable()
                .AnyAsync(g => g.Id == id);
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET LATEST
        // ────────────────────────────────────────────────────────────────────────

        public async Task<SemesterGPA?> GetLatestByStudentIdAsync(string studentId)
        {
            return await BuildBaseQuery()
                .Where(g => g.StudentId == studentId)
                .OrderByDescending(g => g.StudyYear.StartYear)
                .ThenByDescending(g => g.Semester.Title)
                .FirstOrDefaultAsync();
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET CUMULATIVE GPA
        // ────────────────────────────────────────────────────────────────────────

        public async Task<decimal> GetCumulativeGPAAsync(string studentId)
        {
            var gpas = await BuildBaseQuery()
                .Where(g => g.StudentId == studentId)
                .ToListAsync();

            if (!gpas.Any())
                return 0;

            decimal totalWeightedPoints = 0;
            int totalCreditHours = 0;

            foreach (var gpa in gpas)
            {
                totalWeightedPoints += gpa.GPA * gpa.TotalCreditHours;
                totalCreditHours += gpa.TotalCreditHours;
            }

            if (totalCreditHours == 0)
                return 0;

            return Math.Round(totalWeightedPoints / totalCreditHours, 2);
        }

        // ────────────────────────────────────────────────────────────────────────
        // PRIVATE HELPERS
        // ────────────────────────────────────────────────────────────────────────

        private IQueryable<SemesterGPA> BuildBaseQuery()
        {
            return GetQueryable()
                .Include(g => g.Student)
                    .ThenInclude(s => s.User)
                .Include(g => g.Student)
                    .ThenInclude(s => s.Department)
                .Include(g => g.Semester)
                .Include(g => g.StudyYear)
                .AsNoTracking();
        }

        private IQueryable<SemesterGPA> ApplySemesterFilters(
            IQueryable<SemesterGPA> query,
            SemesterGPAFilterInSemesterQueries? filter)
        {
            if (filter == null) return query;

            if (filter.StudyYearId.HasValue)
                query = query.Where(g => g.StudyYearId == filter.StudyYearId.Value);

            if (filter.DepartmentId.HasValue)
                query = query.Where(g => g.Student.DepartmentId == filter.DepartmentId.Value);

            if (filter.MinGPA.HasValue)
                query = query.Where(g => g.GPA >= filter.MinGPA.Value);

            if (filter.MaxGPA.HasValue)
                query = query.Where(g => g.GPA <= filter.MaxGPA.Value);

            if (filter.MinCreditHours.HasValue)
                query = query.Where(g => g.TotalCreditHours >= filter.MinCreditHours.Value);

            if (filter.MaxCreditHours.HasValue)
                query = query.Where(g => g.TotalCreditHours <= filter.MaxCreditHours.Value);

            if (filter.CalculatedFrom.HasValue)
                query = query.Where(g => g.CalculatedAt >= filter.CalculatedFrom.Value);

            if (filter.CalculatedTo.HasValue)
                query = query.Where(g => g.CalculatedAt <= filter.CalculatedTo.Value);

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                var searchTerm = filter.SearchTerm.ToLower();
                query = query.Where(g =>
                    g.Student.User.Name.ToLower().Contains(searchTerm) ||
                    g.Student.AcademicCode.ToLower().Contains(searchTerm) ||
                    g.Student.User.Email.ToLower().Contains(searchTerm));
            }

            return query;
        }

        private IQueryable<SemesterGPA> ApplyStudyYearFilters(
            IQueryable<SemesterGPA> query,
            semesterGPAStudyYearQueries? filter)
        {
            if (filter == null) return query;

            if (filter.DepartmentId.HasValue)
                query = query.Where(g => g.Student.DepartmentId == filter.DepartmentId.Value);

            if (filter.MinGPA.HasValue)
                query = query.Where(g => g.GPA >= filter.MinGPA.Value);

            if (filter.MaxGPA.HasValue)
                query = query.Where(g => g.GPA <= filter.MaxGPA.Value);

            if (filter.MinCreditHours.HasValue)
                query = query.Where(g => g.TotalCreditHours >= filter.MinCreditHours.Value);

            if (filter.MaxCreditHours.HasValue)
                query = query.Where(g => g.TotalCreditHours <= filter.MaxCreditHours.Value);

            if (filter.SemesterTitle.HasValue)
                query = query.Where(g => g.Semester.Title == filter.SemesterTitle.Value);


            if (filter.CalculatedFrom.HasValue)
                query = query.Where(g => g.CalculatedAt >= filter.CalculatedFrom.Value);

            if (filter.CalculatedTo.HasValue)
                query = query.Where(g => g.CalculatedAt <= filter.CalculatedTo.Value);

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                var searchTerm = filter.SearchTerm.ToLower();
                query = query.Where(g =>
                    g.Student.User.Name.ToLower().Contains(searchTerm) ||
                    g.Student.AcademicCode.ToLower().Contains(searchTerm) ||
                    g.Student.User.Email.ToLower().Contains(searchTerm));
            }

            return query;
        }

        // ✅ NEW: Apply all filters from SemesterGPAFilterQueries
        private IQueryable<SemesterGPA> ApplyAllFilters(
            IQueryable<SemesterGPA> query,
            SemesterGPAFilterQueries? filter)
        {
            if (filter == null) return query;        

            if (filter.DepartmentId.HasValue)
                query = query.Where(g => g.Student.DepartmentId == filter.DepartmentId.Value);

            if (filter.StudyYearId.HasValue)
                query = query.Where(g => g.StudyYearId == filter.StudyYearId.Value);

            if (filter.SemesterTitle.HasValue)
                query = query.Where(g => g.Semester.Title == filter.SemesterTitle.Value);

            // GPA filters
            if (filter.MinGPA.HasValue)
                query = query.Where(g => g.GPA >= filter.MinGPA.Value);

            if (filter.MaxGPA.HasValue)
                query = query.Where(g => g.GPA <= filter.MaxGPA.Value);

            // Credit Hours filters
            if (filter.MinCreditHours.HasValue)
                query = query.Where(g => g.TotalCreditHours >= filter.MinCreditHours.Value);

            if (filter.MaxCreditHours.HasValue)
                query = query.Where(g => g.TotalCreditHours <= filter.MaxCreditHours.Value);

            // Date filters
            if (filter.CalculatedFrom.HasValue)
                query = query.Where(g => g.CalculatedAt >= filter.CalculatedFrom.Value);

            if (filter.CalculatedTo.HasValue)
                query = query.Where(g => g.CalculatedAt <= filter.CalculatedTo.Value);

            // Search term
            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                var searchTerm = filter.SearchTerm.ToLower();
                query = query.Where(g =>
                    g.Student.User.Name.ToLower().Contains(searchTerm) ||
                    g.Student.AcademicCode.ToLower().Contains(searchTerm) ||
                    g.Student.User.Email.ToLower().Contains(searchTerm));
            }

            return query;
        }

        // ✅ NEW: Sorting for SemesterGPAFilterQueries
        private IQueryable<SemesterGPA> ApplySortingForFilterQueries(
            IQueryable<SemesterGPA> query,
            SemesterGPAFilterQueries? filter)
        {
            if (filter == null) return query.OrderByDescending(g => g.CalculatedAt);

            return filter.SortBy?.ToLower() switch
            {
                "student" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(g => g.Student.User.Name)
                    : query.OrderByDescending(g => g.Student.User.Name),
                "academiccode" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(g => g.Student.AcademicCode)
                    : query.OrderByDescending(g => g.Student.AcademicCode),
                "gpa" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(g => g.GPA)
                    : query.OrderByDescending(g => g.GPA),
                "credithours" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(g => g.TotalCreditHours)
                    : query.OrderByDescending(g => g.TotalCreditHours),
                "semester" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(g => g.Semester.Title)
                    : query.OrderByDescending(g => g.Semester.Title),
                "studyyear" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(g => g.StudyYear.StartYear)
                    : query.OrderByDescending(g => g.StudyYear.StartYear),
                "department" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(g => g.Student.Department.Name)
                    : query.OrderByDescending(g => g.Student.Department.Name),
                "calculatedat" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(g => g.CalculatedAt)
                    : query.OrderByDescending(g => g.CalculatedAt),
                _ => query.OrderByDescending(g => g.CalculatedAt)
            };
        }

        private IQueryable<SemesterGPA> ApplySorting(
            IQueryable<SemesterGPA> query,
            SearchablePaginationQuery? filter)
        {
            if (filter == null) return query.OrderByDescending(g => g.CalculatedAt);

            return filter.SortBy?.ToLower() switch
            {
                "student" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(g => g.Student.User.Name)
                    : query.OrderByDescending(g => g.Student.User.Name),
                "academiccode" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(g => g.Student.AcademicCode)
                    : query.OrderByDescending(g => g.Student.AcademicCode),
                "gpa" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(g => g.GPA)
                    : query.OrderByDescending(g => g.GPA),
                "credithours" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(g => g.TotalCreditHours)
                    : query.OrderByDescending(g => g.TotalCreditHours),
                "semester" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(g => g.Semester.Title)
                    : query.OrderByDescending(g => g.Semester.Title),
                "studyyear" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(g => g.StudyYear.StartYear)
                    : query.OrderByDescending(g => g.StudyYear.StartYear),
                "calculatedat" => filter.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(g => g.CalculatedAt)
                    : query.OrderByDescending(g => g.CalculatedAt),
                _ => query.OrderByDescending(g => g.CalculatedAt)
            };
        }
    }
}