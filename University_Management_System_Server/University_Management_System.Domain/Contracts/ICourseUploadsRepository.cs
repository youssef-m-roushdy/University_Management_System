
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Domain.Contracts
{
    public interface ICourseUploadsRepository : IGenericRepository<CourseUpload, int>
    {
        Task<IEnumerable<CourseUpload>> GetByCourseIdAsync(int courseId);
    }
}
