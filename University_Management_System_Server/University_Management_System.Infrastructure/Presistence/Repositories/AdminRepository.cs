using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Identity;

namespace University_Management_System.Infrastructure.Presistence.Repositories
{
    public class AdminRepository : GenericRepository<Admin, string>, IAdminRepository
    {
        public AdminRepository(UniversityDbContext dbContext) : base(dbContext)
        {
        }
        
    }
}