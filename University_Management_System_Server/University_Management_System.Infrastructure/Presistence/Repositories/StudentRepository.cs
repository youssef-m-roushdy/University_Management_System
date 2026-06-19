using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Identity;

namespace University_Management_System.Infrastructure.Presistence.Repositories
{
    public class StudentRepository : GenericRepository<Student, string>, IStudentRepository
    {
        public StudentRepository(UniversityDbContext dbContext) : base(dbContext)
        {
            
        }
       
    }
}