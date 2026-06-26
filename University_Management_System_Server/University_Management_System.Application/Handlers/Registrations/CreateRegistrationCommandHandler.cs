using AutoMapper;
using MediatR;
using University_Management_System.Application.Commands.Registrations;
using University_Management_System.Application.Dtos.RegistrationDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Enums;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Registrations
{
    public class CreateRegistrationCommandHandler : IRequestHandler<CreateRegistrationCommand, RegistrationDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateRegistrationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RegistrationDto> Handle(CreateRegistrationCommand request, CancellationToken cancellationToken)
        {
            // ─── 1. Validate Student exists ──────────────────────────────────
            var student = await _unitOfWork.Students.GetByIdAsync(request.StudentId);
            if (student == null)
                throw new NotFoundException($"Student with ID '{request.StudentId}' not found.");

            // ─── 2. Validate Student is active ──────────────────────────────
            if (!student.User.IsActive)
                throw new ValidationException(new List<string> {
                    $"Student account is not active. Please contact administration."
                });

            // ─── 3. Validate Course exists ──────────────────────────────────
            var course = await _unitOfWork.Courses.GetByIdAsync(request.Dto.CourseId);
            if (course == null)
                throw new NotFoundException($"Course with ID '{request.Dto.CourseId}' not found.");

            // ─── 4. Validate Course is active ──────────────────────────────
            if (course.Status == CourseStatus.Closed)
                throw new ValidationException(new List<string> {
                    $"Course '{course.Code}' is not open to register."
                });

            // ─── 5. Validate Semester exists ──────────────────────────────────
            var semester = await _unitOfWork.Semesters.GetByIdAsync(request.Dto.SemesterId);
            if (semester == null)
                throw new NotFoundException($"Semester with ID '{request.Dto.SemesterId}' not found.");

            // ─── 6. Validate Study Year exists ──────────────────────────────
            var studyYear = await _unitOfWork.StudyYears.GetByIdAsync(request.Dto.StudyYearId);
            if (studyYear == null)
                throw new NotFoundException($"Study Year with ID '{request.Dto.StudyYearId}' not found.");

            // ─── 7. Check if student is already registered in this course ────
            var existingRegistration = await _unitOfWork.Registrations
                .GetByStudentAndCourseAsync(request.StudentId, request.Dto.CourseId, request.Dto.StudyYearId);
            
            if (existingRegistration != null)
                throw new ValidationException(new List<string> {
                    $"Student is already registered in course '{course.Code}' for this study year."
                });

            // ─── 8. Check if student has prerequisites ──────────────────────
            var prerequisites = await _unitOfWork.Courses.GetPrerequisitesAsync(request.Dto.CourseId);
            if (prerequisites.Any())
            {
                var passedCourses = await _unitOfWork.Registrations
                    .GetStudentPassedCoursesAsync(request.StudentId);
                
                var passedCourseIds = passedCourses.Select(p => p.CourseId).ToHashSet();
                var missingPrerequisites = prerequisites
                    .Where(p => !passedCourseIds.Contains(p.Id))
                    .ToList();

                if (missingPrerequisites.Any())
                {
                    var missingCodes = string.Join(", ", missingPrerequisites.Select(p => p.Code));
                    throw new ValidationException(new List<string> {
                        $"Student has not completed prerequisites: {missingCodes}"
                    });
                }
            }

            // ─── 9. Check if course is open for registration ──────────────────
            if (course.Status != CourseStatus.Opened)
                throw new ValidationException(new List<string> {
                    $"Course '{course.Code}' is not open for registration. Current status: {course.Status}."
                });

            // ─── 10. Credit Validation ──────────────────────────────────────
            var courseCredits = course.Credits;
            
            // Check if student has enough allowed credits
            if (student.AllowedCredits < courseCredits)
                throw new ValidationException(new List<string> {
                    $"Student does not have enough allowed credits. Available: {student.AllowedCredits}, Required: {courseCredits}."
                });

            // Check if student would exceed semester credit limit
            var currentSemesterCredits = await _unitOfWork.Registrations
                .GetStudentCreditHoursAsync(request.StudentId, request.Dto.SemesterId);
            
            var maxAllowedCredits = student.AllowedCredits + student.TotalCredits;
            
            if (currentSemesterCredits + courseCredits > maxAllowedCredits)
                throw new ValidationException(new List<string> {
                    $"Student would exceed allowed credits for this semester. " +
                    $"Current: {currentSemesterCredits}, Adding: {courseCredits}, " +
                    $"Max: {maxAllowedCredits}."
                });

            // ─── 11. Check for scheduling conflicts ──────────────────────────
            var hasConflict = await CheckSchedulingConflictAsync(
                request.StudentId, 
                request.Dto.SemesterId, 
                request.Dto.CourseId);

            if (hasConflict)
                throw new ValidationException(new List<string> {
                    $"Course '{course.Code}' conflicts with another registered course in the same semester."
                });

            // ─── 12. Create Registration ──────────────────────────────────────
            var registration = new Registration
            {
                StudentId = request.StudentId,
                CourseId = request.Dto.CourseId,
                SemesterId = request.Dto.SemesterId,
                StudyYearId = request.Dto.StudyYearId,
                Status = RegistrationStatus.Pending,
                Progress = CourseProgress.NotStarted,
                IsPassed = false,
                RegisteredAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Registrations.AddAsync(registration);
            
            // ─── 13. ✅ DECREASE student's allowed credits ──────────────────
            student.AllowedCredits -= courseCredits;
            student.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.Students.UpdateAsync(student);
            
            await _unitOfWork.SaveChangesAsync();

            // ─── 14. Get created registration with details ────────────────────
            var created = await _unitOfWork.Registrations
                .GetByStudentAndCourseAsync(request.StudentId, request.Dto.CourseId, request.Dto.StudyYearId);

            // ─── 15. Return DTO ──────────────────────────────────────────────
            var result = _mapper.Map<RegistrationDto>(created);
            result.Status = RegistrationStatus.Pending;
            result.Progress = CourseProgress.NotStarted;
            
            return result;
        }

        private async Task<bool> CheckSchedulingConflictAsync(string studentId, int semesterId, int courseId)
        {
            // Get all registered courses for this student in the semester
            var existingRegistrations = await _unitOfWork.Registrations
                .GetStudentSemesterCoursesAsync(studentId, 0, semesterId);

            // Get the course being registered
            var newCourse = await _unitOfWork.Courses.GetByIdAsync(courseId);
            
            // Check for time conflicts (if you have schedule data)
            // This is a placeholder - implement based on your schedule model
            // For now, return false (no conflicts)
            return false;
        }
    }
}