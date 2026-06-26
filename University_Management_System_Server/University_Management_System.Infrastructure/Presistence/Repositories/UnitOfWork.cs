using System.Collections.Concurrent;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities;

namespace University_Management_System.Infrastructure.Presistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UniversityDbContext _dbContext;
        private readonly ConcurrentDictionary<string, object> _repositories;
        private IDepartmentRepository?       _departments;
        private ICourseRepository?           _courses;
        private IAcademicScheduleRepository? _academicSchedules;
        private IFeeRepository?              _fees;
        private IStudyYearRepository?        _studyYears;
        private IRegistrationRepository?     _registrations;
        private ICourseUploadsRepository?    _courseUploads;
        private ISemesterRepository?         _semesters;
        private IStudentStudyYearRepository?    _studentStudyYears;
        private ISpecializationRepository?   _specializations;   // ← added
        private IStudentRepository?         _students;
        private IInstructorRepository?      _instructors;
        private IAssistantRepository?        _assistants;
        private IAdminRepository?            _admins;
        private ICoursePrerequisiteRepository     _coursePrerequisite;
        private IDepartmentCourseRepository    _departmentCourse;
        private ISpecializationCourseRepository   _specializationCourse;

        public UnitOfWork(UniversityDbContext dbContext)
        {
            _dbContext    = dbContext;
            _repositories = new();
        }

        public IDepartmentRepository Departments
            => _departments ??= new DepartmentRepository(_dbContext);

        public ICourseRepository Courses
            => _courses ??= new CourseRepository(_dbContext);

        public IAcademicScheduleRepository AcademicSchedules
            => _academicSchedules ??= new AcademicScheduleRepository(_dbContext);

        public IFeeRepository Fees
            => _fees ??= new FeeRepository(_dbContext);

        public IStudyYearRepository StudyYears
            => _studyYears ??= new StudyYearRepository(_dbContext);

        public IRegistrationRepository Registrations
            => _registrations ??= new RegistrationRepository(_dbContext);

        public ICourseUploadsRepository CourseUploads
            => _courseUploads ??= new CourseUploadsRepository(_dbContext);

        public ISemesterRepository Semesters
            => _semesters ??= new SemesterRepository(_dbContext);

        public IStudentStudyYearRepository StudentStudyYears
            => _studentStudyYears ??= new StudentStudyYearRepository(_dbContext);

        public ISpecializationRepository Specializations                    // ← added
            => _specializations ??= new SpecializationRepository(_dbContext);
        
        public IStudentRepository Students
            => _students ??= new StudentRepository(_dbContext);

        public IInstructorRepository Instructors
            => _instructors ??= new InstructorRepository(_dbContext);
        
        public IAssistantRepository Assistants
            => _assistants ??= new AssistantRepository(_dbContext);
        
        public IAdminRepository Admins
            => _admins ??= new AdminRepository(_dbContext);
        
        public ICoursePrerequisiteRepository CoursePrerequisites
            => _coursePrerequisite ??= new CoursePrerequisiteRepository(_dbContext);

        public IDepartmentCourseRepository DepartmentCourses
            => _departmentCourse ??= new DepartmentCourseRepository(_dbContext);

        public ISpecializationCourseRepository SpecializationCourses
            => _specializationCourse ??= new SpecializationCourseRepository(_dbContext);

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : BaseEntities<TKey>
            => (IGenericRepository<TEntity, TKey>)_repositories.GetOrAdd(
                typeof(TEntity).Name,
                _ => new GenericRepository<TEntity, TKey>(_dbContext));

        public async Task<int> SaveChangesAsync()
            => await _dbContext.SaveChangesAsync();

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}