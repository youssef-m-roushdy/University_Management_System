using System.Runtime.InteropServices;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Enums;
using University_Management_System.Domain.Queries;

namespace University_Management_System.Domain.Contracts
{
    public interface ISemesterRepository : IGenericRepository<Semester, int>
    {
        Task<bool> IsActiveSemesterAsync(int semesterId);
        Task<bool> IsSemesterBelongsToStudyYearAsync(int semesterId, int studyYearId);
        Task<bool> SemesterTitleExistsInStudyYearAsync(int studyYearId, SemesterEnum title);

        // the year is have 3 or 2 semesters, so we dont need to paginate the response
        Task<IEnumerable<Semester>> GetSemestersByStudyYearIdAsync(int studyYearId);
        Task<Semester> CreateStudyYearSemesterAsync(int studyYearId, Semester semester);
        
       
    }
}