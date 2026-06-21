using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Queries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Infrastructure.Presistence.Repositories
{
    public class StudyYearRepository : GenericRepository<StudyYear, int>, IStudyYearRepository
    {
        public StudyYearRepository(UniversityDbContext dbContext) : base(dbContext)
        {
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET BY SPECIFIC CRITERIA
        // ────────────────────────────────────────────────────────────────────────

        public async Task<StudyYear?> GetCurrentStudyYearAsync()
        {
            return await _dbContext.StudyYears
                .Include(sy => sy.Semesters)
                .FirstOrDefaultAsync(sy => sy.IsCurrent == true);
        }

        public async Task<StudyYear?> GetStudyYearByYearRangeAsync(int startYear, int endYear)
        {
            return await _dbContext.StudyYears
                .FirstOrDefaultAsync(sy => sy.StartYear == startYear && sy.EndYear == endYear);
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET COLLECTIONS
        // ────────────────────────────────────────────────────────────────────────

        public async Task<IEnumerable<StudyYear>> GetStudyYearsByYearRangeAsync(int startYear, int endYear)
        {
            return await _dbContext.StudyYears
                .Where(sy => sy.StartYear >= startYear && sy.EndYear <= endYear)
                .OrderBy(sy => sy.StartYear)
                .ToListAsync();
        }

        public async Task<IEnumerable<StudyYear>> GetStudyYearsWithSemestersAsync()
        {
            return await _dbContext.StudyYears
                .Include(sy => sy.Semesters)
                .Where(sy => sy.Semesters.Any())
                .ToListAsync();
        }

        public async Task<IEnumerable<StudyYear>> GetStudyYearsWithRegistrationsAsync()
        {
            return await _dbContext.StudyYears
                .Include(sy => sy.Registrations)
                .Where(sy => sy.Registrations.Any())
                .ToListAsync();
        }

        public async Task<IEnumerable<StudyYear>> GetStudyYearsBetweenYearsAsync(int minYear, int maxYear)
        {
            return await _dbContext.StudyYears
                .Where(sy => sy.StartYear >= minYear && sy.EndYear <= maxYear)
                .OrderByDescending(sy => sy.StartYear)
                .ToListAsync();
        }

        // ────────────────────────────────────────────────────────────────────────
        // CHECK EXISTENCE
        // ────────────────────────────────────────────────────────────────────────

        public async Task<bool> HasRegistrationsAsync(int studyYearId)
        {
            return await _dbContext.StudyYears
                .AnyAsync(sy => sy.Id == studyYearId && sy.Registrations.Any());
        }

        public async Task<bool> HasSemestersAsync(int studyYearId)
        {
            return await _dbContext.StudyYears
                .AnyAsync(sy => sy.Id == studyYearId && sy.Semesters.Any());
        }

        public async Task<bool> HasFeesAsync(int studyYearId)
        {
            return await _dbContext.StudyYears
                .AnyAsync(sy => sy.Id == studyYearId && sy.Fees.Any());
        }

        public async Task<bool> StudyYearExistsAsync(int startYear, int endYear)
        {
            return await _dbContext.StudyYears
                .AnyAsync(sy => sy.StartYear == startYear && sy.EndYear == endYear);
        }

        public async Task<bool> IsStudyYearCurrentAsync(int studyYearId)
        {
            var studyYear = await GetByIdAsync(studyYearId);
            return studyYear?.IsCurrent ?? false;
        }

        // ────────────────────────────────────────────────────────────────────────
        // COUNTS
        // ────────────────────────────────────────────────────────────────────────

        public async Task<int> GetStudentCountAsync(int studyYearId)
        {
            return await _dbContext.StudentStudyYears
                .Where(ssy => ssy.StudyYearId == studyYearId)
                .Select(ssy => ssy.StudentId)
                .Distinct()
                .CountAsync();
        }

        public async Task<int> GetSemesterCountAsync(int studyYearId)
        {
            return await _dbContext.Semesters
                .CountAsync(s => s.StudyYearId == studyYearId);
        }

        public async Task<int> GetRegistrationCountAsync(int studyYearId)
        {
            return await _dbContext.Registrations
                .CountAsync(r => r.StudyYearId == studyYearId);
        }

        // ────────────────────────────────────────────────────────────────────────
        // BULK OPERATIONS
        // ────────────────────────────────────────────────────────────────────────

        public async Task<IEnumerable<StudyYear>> GetStudyYearsWithDetailsAsync()
        {
            return await _dbContext.StudyYears
                .Include(sy => sy.Semesters)
                .Include(sy => sy.Fees)
                .Include(sy => sy.Registrations)
                .Include(sy => sy.StudentStudyYears)
                    .ThenInclude(ssy => ssy.Student)
                .Include(sy => sy.AcademicSchedules)
                .OrderByDescending(sy => sy.StartYear)
                .ToListAsync();
        }

        public async Task<IEnumerable<StudyYear>> GetFilteredStudyYearsAsync(StudyYearQueries query)
        {
            var queryable = GetQueryable();

            // Apply filters
            if (query.StartYear.HasValue)
                queryable = queryable.Where(sy => sy.StartYear == query.StartYear.Value);

            if (query.EndYear.HasValue)
                queryable = queryable.Where(sy => sy.EndYear == query.EndYear.Value);

            if (query.IsCurrent.HasValue)
                queryable = queryable.Where(sy => sy.IsCurrent == query.IsCurrent.Value);

            if (query.MinYear.HasValue)
                queryable = queryable.Where(sy => sy.StartYear >= query.MinYear.Value);

            if (query.MaxYear.HasValue)
                queryable = queryable.Where(sy => sy.EndYear <= query.MaxYear.Value);

            if (query.HasSemesters.HasValue)
            {
                if (query.HasSemesters.Value)
                    queryable = queryable.Where(sy => sy.Semesters.Any());
                else
                    queryable = queryable.Where(sy => !sy.Semesters.Any());
            }

            if (query.HasRegistrations.HasValue)
            {
                if (query.HasRegistrations.Value)
                    queryable = queryable.Where(sy => sy.Registrations.Any());
                else
                    queryable = queryable.Where(sy => !sy.Registrations.Any());
            }

            // Apply sorting
            queryable = query.SortBy?.ToLower() switch
            {
                "startyear" => query.SortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(sy => sy.StartYear)
                    : queryable.OrderByDescending(sy => sy.StartYear),
                "endyear" => query.SortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(sy => sy.EndYear)
                    : queryable.OrderByDescending(sy => sy.EndYear),
                "iscurrent" => query.SortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(sy => sy.IsCurrent)
                    : queryable.OrderByDescending(sy => sy.IsCurrent),
                _ => queryable.OrderByDescending(sy => sy.StartYear)
            };

            // Apply pagination
            queryable = queryable
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize);

            return await queryable.ToListAsync();
        }

        // ────────────────────────────────────────────────────────────────────────
        // HISTORY
        // ────────────────────────────────────────────────────────────────────────

        public async Task<IEnumerable<StudyYear>> GetPreviousStudyYearsAsync(int count)
        {
            var currentYear = DateTime.UtcNow.Year;
            
            return await _dbContext.StudyYears
                .Where(sy => sy.EndYear < currentYear)
                .OrderByDescending(sy => sy.EndYear)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<StudyYear>> GetUpcomingStudyYearsAsync(int count)
        {
            var currentYear = DateTime.UtcNow.Year;
            
            return await _dbContext.StudyYears
                .Where(sy => sy.StartYear >= currentYear)
                .OrderBy(sy => sy.StartYear)
                .Take(count)
                .ToListAsync();
        }
    }
}