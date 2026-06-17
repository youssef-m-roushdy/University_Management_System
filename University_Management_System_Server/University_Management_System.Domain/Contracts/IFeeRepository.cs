using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Domain.Contracts
{
    public interface IFeeRepository : IGenericRepository<Fee, int>
    {
        Task<IEnumerable<Fee>> GetFeesOfDepartmentForStudyYear(int departmentId, int studyYearId);
      

        Task<IEnumerable<Fee>> GetFeesOfStudyYear(int studyYearId);
       
    }
}
