using University_Management_System.Domain.Entities;

namespace University_Management_System.Domain.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IDepartmentRepository Departments { get; }
        ICourseRepository Courses { get; }
        IAcademicScheduleRepository AcademicSchedules { get; }
        IFeeRepository Fees { get; }
        IStudyYearRepository StudyYears { get; }
        IRegistrationRepository Registrations { get; }
        ICourseUploadsRepository CourseUploads { get; }
        ISemesterRepository Semesters { get; }
        IStudentStudyYearRepository StudentStudyYears { get; }
        ISpecializationRepository Specializations { get; }   // ← added
        IStudentRepository Students { get; }
        IInstructorRepository Instructors { get; }
        IAssistantRepository Assistants { get; }
        IAdminRepository Admins { get; }
        ICoursePrerequisiteRepository CoursePrerequisites { get; }
        IDepartmentCourseRepository DepartmentCourses { get; }
        ISpecializationCourseRepository SpecializationCourses { get; }

        Task<int> SaveChangesAsync();

        IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntities<TKey>;
    }
}