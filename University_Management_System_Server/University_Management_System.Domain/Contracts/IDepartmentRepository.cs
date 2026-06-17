using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Domain.Contracts
{
    public interface IDepartmentRepository : IGenericRepository<Department, int>
    {
        Task<Department?> GetByNameAsync(string name);
        Task<Department?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Department>> GetAllWithDetailsAsync();
    }
}
