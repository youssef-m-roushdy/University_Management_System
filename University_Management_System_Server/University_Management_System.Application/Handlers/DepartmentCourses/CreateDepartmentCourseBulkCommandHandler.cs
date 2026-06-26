using AutoMapper;
using MediatR;
using University_Management_System.Application.Commands.DepartmentCourses;
using University_Management_System.Application.Dtos.DepartmentCourseDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.DepartmentCourses
{
    public class CreateDepartmentCourseBulkCommandHandler : IRequestHandler<CreateDepartmentCourseBulkCommand, IEnumerable<DepartmentCourseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateDepartmentCourseBulkCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DepartmentCourseDto>> Handle(
            CreateDepartmentCourseBulkCommand request,
            CancellationToken cancellationToken)
        {
            // ─── 1. Validate Department exists ──────────────────────────────────
            var department = await _unitOfWork.Departments.GetByIdAsync(request.Dto.DepartmentId);
            if (department == null)
                throw new NotFoundException($"Department with ID '{request.Dto.DepartmentId}' not found.");

            // ─── 2. Validate all courses exist ──────────────────────────────────
            var courseIds = request.Dto.CourseIds.Distinct().ToList();
            var existingCourses = new List<Course>();
            var errors = new List<string>();

            foreach (var courseId in courseIds)
            {
                var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
                if (course == null)
                {
                    errors.Add($"Course with ID '{courseId}' not found.");
                    continue;
                }
                existingCourses.Add(course);

                // Check if mapping already exists
                var exists = await _unitOfWork.DepartmentCourses.ExistsAsync(
                    request.Dto.DepartmentId,
                    courseId);
                
                if (exists)
                {
                    errors.Add($"Course '{course.Code}' is already assigned to department '{department.Name}'.");
                }
            }

            if (errors.Any())
                throw new ValidationException(errors);

            // ─── 3. Create all mappings ──────────────────────────────────────────
            var mappings = new List<DepartmentCourse>();
            var createdCourses = new List<Course>();

            foreach (var course in existingCourses)
            {
                var mapping = new DepartmentCourse
                {
                    DepartmentId = request.Dto.DepartmentId,
                    CourseId = course.Id,
                    Role = request.Dto.Role,
                    CreatedAt = DateTime.UtcNow
                };
                mappings.Add(mapping);
                createdCourses.Add(course);
            }

            if (mappings.Any())
            {
                await _unitOfWork.DepartmentCourses.AddRangeAsync(mappings);
                await _unitOfWork.SaveChangesAsync();
            }

            // ─── 4. Get full details ──────────────────────────────────────────
            var createdMappings = await _unitOfWork.DepartmentCourses
                .GetByDepartmentIdWithDetailsAsync(request.Dto.DepartmentId);
            
            var result = createdMappings
                .Where(dc => request.Dto.CourseIds.Contains(dc.CourseId))
                .ToList();

            return _mapper.Map<IEnumerable<DepartmentCourseDto>>(result);
        }
    }
}