using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Enums;
using University_Management_System.Domain.Queries;

namespace University_Management_System.Domain.Contracts
{
    public interface ICourseRepository : IGenericRepository<Course, int>
    {
        Task<IEnumerable<Course>> GetFilteredCoursesAsync(CourseQueries query);
        Task<(IEnumerable<Course> Data, int TotalCount)> GetFilteredCoursesWithPaginationAsync(CourseQueries query);
        Task<Course?> GetCourseUplaodsAsync(int id);
        Task<IEnumerable<Course>> GetDepartmentCoursesAsync(int departmentId, DepartmentCourseQueries query);
        Task<(IEnumerable<Course> Data, int TotalCount)> GetDepartmentCoursesWithPaginationAsync(int departmentId, DepartmentCourseQueries query);
        Task<IEnumerable<Course>> GetCourseDependenciesAsync(int courseId);
        Task<IEnumerable<Course>> GetCoursePrerequisitesAsync(int courseId);
        Task<IEnumerable<Course>> GetPassedCoursesByUserAsync(string userId);
        Task<IEnumerable<Course>> GetOpenCoursesAsync();
        Task UpdateCourseStatusAsync(int courseId, CourseStatus newStatus);
        Task<IEnumerable<Course>> GetAllPrerequisitesForOpenCoursesAsync();
        Task<IEnumerable<CoursePrerequisite>> GetCoursePrerequisiteMappingsForOpenCoursesAsync();
    }
}

