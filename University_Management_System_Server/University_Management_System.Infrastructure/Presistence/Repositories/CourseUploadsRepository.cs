using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace University_Management_System.Infrastructure.Presistence.Repositories
{
    public class CourseUploadsRepository : GenericRepository<CourseUpload, int>, ICourseUploadsRepository
    {
        public CourseUploadsRepository(UniversityDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<CourseUpload>> GetByCourseIdAsync(int courseId)
        {
            return await _dbContext.CourseUploads
                .Where(cu => cu.CourseId == courseId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<CourseUpload>> GetByUserIdAsync(string userId)
        {
            return await _dbContext.CourseUploads
                .Where(cu => cu.UploadedByUserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
