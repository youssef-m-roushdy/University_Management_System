using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Entities.Identity;

namespace University_Management_System.Domain.Contracts
{
    public interface IStudentRepository : IGenericRepository<Student, string>
    {
        Task<IEnumerable<Student>> GetSpecificUngraduatedStudentsAsync(IEnumerable<string> studentIds);
        Task<Student?> GetStudentByAcademicCode(string academicCode);
    }
}