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
        private IUserStudyYearRepository?    _userStudyYears;
        private ISpecializationRepository?   _specializations;   // ← added

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

        public IUserStudyYearRepository UserStudyYears
            => _userStudyYears ??= new UserStudyYearRepository(_dbContext);

        public ISpecializationRepository Specializations                    // ← added
            => _specializations ??= new SpecializationRepository(_dbContext);

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