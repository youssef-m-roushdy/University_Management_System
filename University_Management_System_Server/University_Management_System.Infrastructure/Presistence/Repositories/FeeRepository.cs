using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace University_Management_System.Infrastructure.Presistence.Repositories
{
    public class FeeRepository : GenericRepository<Fee, int>, IFeeRepository
    {
        public FeeRepository(UniversityDbContext dbContext) : base(dbContext)
        {
        }


        public async Task<IEnumerable<Fee>> GetFeesOfDepartmentForStudyYear(int departmentId, int studyYearId)
        {
            return await _dbContext.Fees
                .Where(f => f.DepartmentId == departmentId && f.StudyYearId == studyYearId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Fee>> GetFeesOfStudyYear(int studyYearId)
        {
            return await _dbContext.Fees
                .Where(f => f.StudyYearId == studyYearId)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
