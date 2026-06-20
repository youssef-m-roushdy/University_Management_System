using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Infrastructure.Presistence.Repositories
{
    public class StudentRepository : GenericRepository<Student, string>, IStudentRepository
    {
        public StudentRepository(UniversityDbContext dbContext) : base(dbContext)
        {
            
        }

        public async Task<IEnumerable<Student>> GetSpecificUngraduatedStudentsAsync(IEnumerable<string> academicCodes)
        {
            return await _dbContext.Students
                .Where(s => academicCodes.Contains(s.AcademicCode) && s.Level != Levels.Graduate)
                .ToListAsync();
        }

        public async Task<Student?> GetStudentByAcademicCode(string academicCode)
        {
            // now return user or null if not found, instead of throwing exception, because we will handle the exception in the handler
            return await _dbContext.Students.FirstOrDefaultAsync(s => s.AcademicCode == academicCode);
        }
       
    }
}