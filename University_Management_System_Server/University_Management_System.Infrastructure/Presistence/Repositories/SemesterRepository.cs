using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;
using University_Management_System.Domain.Queries;
using University_Management_System.Application.Dtos.SemesterDtos;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Infrastructure.Presistence.Repositories
{
    public class SemesterRepository : GenericRepository<Semester, int>, ISemesterRepository
    {
        public SemesterRepository(UniversityDbContext dbContext) : base(dbContext)
        {

        }
 
        async Task<bool> SemesterTitleExistsInStudyYearAsync(int studyYearId, SemesterEnum title)
        {
            return await _dbContext.Semesters.AnyAsync(s => s.StudyYearId == studyYearId && s.Title == title);
        }

        public async Task<Semester> CreateStudyYearSemesterAsync(int studyYearId, Semester semester)
        {
            semester.StudyYearId = studyYearId;
            await _dbContext.Semesters.AddAsync(semester);
            await _dbContext.SaveChangesAsync();
            return semester;
        }

        public async Task<IEnumerable<Semester>> GetSemestersByStudyYearIdAsync(int studyYearId)
        {
            return await GetQueryable()
                .Where(s => s.StudyYearId == studyYearId)
                .ToListAsync();
        }

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

        Task<bool> ISemesterRepository.SemesterTitleExistsInStudyYearAsync(int studyYearId, SemesterEnum title)
        {
            return SemesterTitleExistsInStudyYearAsync(studyYearId, title);
        }
    }
}