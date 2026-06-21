using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Queries;

namespace University_Management_System.Domain.Contracts
{
    public interface IFeeRepository : IGenericRepository<Fee, int>
    {
        Task<IEnumerable<Fee>> GetFeesOfDepartmentForStudyYear(int departmentId, int studyYearId);
      

        Task<IEnumerable<Fee>> GetFeesOfStudyYear(int studyYearId);
        Task<(IEnumerable<Fee> Data, int TotalCount)> GetByStudyYearIdAsync(
            int studyYearId,
            GetStudyYearNestedQueries query,
            CancellationToken cancellationToken);
       
    }
}
