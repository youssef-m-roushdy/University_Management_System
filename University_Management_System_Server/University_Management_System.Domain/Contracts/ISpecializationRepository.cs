using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Domain.Contracts
{
    public interface ISpecializationRepository : IGenericRepository<Specialization, int>
    {
        // ── Queries ────────────────────────────────────────────────────────────

        /// <summary>All specializations that belong to a given department.</summary>
        Task<IEnumerable<Specialization>> GetByDepartmentIdAsync(int departmentId);

        /// <summary>Single specialization with its department and full course list loaded.</summary>
        Task<Specialization?> GetWithCoursesAsync(int specializationId);

        /// <summary>Single specialization with its department loaded.</summary>
        Task<Specialization?> GetWithDepartmentAsync(int specializationId);

        /// <summary>
        /// Courses linked to a specialization, optionally filtered by role
        /// (Core / Specialization_Core / Elective).
        /// </summary>
        Task<IEnumerable<SpecializationCourse>> GetSpecializationCoursesAsync(
            int specializationId,
            SpecializationCourseRole? role = null);

        /// <summary>Students (Users) enrolled in this specialization.</summary>
        Task<IEnumerable<User>> GetStudentsAsync(int specializationId);

        /// <summary>Check whether a specialization name already exists inside a department.</summary>
        Task<bool> ExistsAsync(string name, int departmentId);

        // ── SpecializationCourse link management ───────────────────────────────

        /// <summary>Get a single SpecializationCourse link by its composite keys.</summary>
        Task<SpecializationCourse?> GetSpecializationCourseAsync(int specializationId, int courseId);

        /// <summary>Add a course to a specialization with a given role.</summary>
        Task AddCourseAsync(SpecializationCourse specializationCourse);

        /// <summary>Remove a course from a specialization.</summary>
        Task RemoveCourseAsync(SpecializationCourse specializationCourse);

        /// <summary>Update the role of an existing SpecializationCourse link.</summary>
        Task UpdateCourseRoleAsync(SpecializationCourse specializationCourse);
    }
}