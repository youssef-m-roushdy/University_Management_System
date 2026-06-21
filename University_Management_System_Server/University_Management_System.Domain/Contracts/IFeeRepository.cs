using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Queries;

namespace University_Management_System.Domain.Contracts
{
    public interface IFeeRepository : IGenericRepository<Fee, int>
    {
        Task<IEnumerable<Fee>> GetFeesOfDepartmentForStudyYear(int departmentId, int studyYearId);
      

        Task<(IEnumerable<Fee> Data, int TotalCount)> GetFeesByStudyYearIdAsync(
            int studyYearId,
            StudyYearFeeQueries query,
            CancellationToken cancellationToken);
       
    }
}
