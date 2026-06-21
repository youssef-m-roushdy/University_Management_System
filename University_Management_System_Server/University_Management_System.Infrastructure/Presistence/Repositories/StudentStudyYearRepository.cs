using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Queries;

namespace University_Management_System.Infrastructure.Presistence.Repositories
{
    public class StudentStudyYearRepository : GenericRepository<StudentStudyYear, int>, IStudentStudyYearRepository
    {
        public StudentStudyYearRepository(UniversityDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<StudentStudyYear>> GetByStudentIdAsync(string studentId)
        {
            return await _dbContext.StudentStudyYears
                .Where(ssy => ssy.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<StudentStudyYear?> GetCurrentByStudentIdAsync(string studentId)
        {
            return await _dbContext.StudentStudyYears
                .FirstOrDefaultAsync(ssy => ssy.StudentId == studentId);
        }

        public async Task<StudentStudyYear?> GetByStudentAndStudyYearAsync(string studentId, int studyYearId)
        {
            return await _dbContext.StudentStudyYears
                .FirstOrDefaultAsync(ssy => ssy.StudentId == studentId && ssy.StudyYearId == studyYearId);
        }

        public async Task<IEnumerable<StudentStudyYear>> GetByStudyYearIdAsync(int studyYearId)
        {
            return await _dbContext.StudentStudyYears
                .Where(ssy => ssy.StudyYearId == studyYearId)
                .ToListAsync();
        }

        public async Task AddRangeAsync(IEnumerable<StudentStudyYear> StudentStudyYears)
        {
            await _dbContext.StudentStudyYears.AddRangeAsync(StudentStudyYears);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<StudentStudyYear>> GetStudyYearsByStudentIdAsync(string studentId)
        {
            return await _dbContext.StudentStudyYears
                .Where(ssy => ssy.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<(IEnumerable<StudentStudyYear> Data, int TotalCount)> GetStudentsOfTheStudyYearByIdAsync(
            int studyYearId,
            StudyYearStudentQueries query,
            CancellationToken cancellationToken)
        {
            var studentStudyYears = GetQueryable()
                .Include(ssy => ssy.Student)
                    .ThenInclude(s => s.User)
                .Where(ssy => ssy.StudyYearId == studyYearId);

            // Check student is Active for this study year
            if(query.IsActive.HasValue)
                studentStudyYears = studentStudyYears.Where(ssy => ssy.IsActive == query.IsActive.Value);

            if (!string.IsNullOrWhiteSpace(query.AcademicCode))
                studentStudyYears = studentStudyYears.Where(ssy => ssy.Student.AcademicCode.Contains(query.AcademicCode));

            if (query.Level.HasValue)
                studentStudyYears = studentStudyYears.Where(ssy => ssy.Level == query.Level.Value);

            if (query.DepartmentId.HasValue)
                studentStudyYears = studentStudyYears.Where(ssy => ssy.Student.DepartmentId == query.DepartmentId.Value);

            if (query.MinGPA.HasValue)
                studentStudyYears = studentStudyYears.Where(ssy => ssy.Student.TotalGPA >= query.MinGPA.Value);

            if (query.MaxGPA.HasValue)
                studentStudyYears = studentStudyYears.Where(ssy => ssy.Student.TotalGPA <= query.MaxGPA.Value);

            if (query.EnrolledFrom.HasValue)
                studentStudyYears = studentStudyYears.Where(ssy => ssy.EnrolledAt >= query.EnrolledFrom.Value);

            if (query.EnrolledTo.HasValue)
                studentStudyYears = studentStudyYears.Where(ssy => ssy.EnrolledAt <= query.EnrolledTo.Value);

            var totalCount = await studentStudyYears.CountAsync(cancellationToken);

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
                _ => studentStudyYears.OrderBy(ssy => ssy.Student.User.Name)
            };

            var result = await studentStudyYears
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);

            return (result, totalCount);
        }

    }
}