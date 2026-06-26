using AutoMapper;
using MediatR;
using University_Management_System.Application.Commands.SpecializationCourses;
using University_Management_System.Application.Dtos.SpecializationCourseDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.SpecializationCourses
{
    public class CreateSpecializationCourseBulkCommandHandler : IRequestHandler<CreateSpecializationCourseBulkCommand, IEnumerable<SpecializationCourseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateSpecializationCourseBulkCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SpecializationCourseDto>> Handle(
            CreateSpecializationCourseBulkCommand request,
            CancellationToken cancellationToken)
        {
            // ─── 1. Validate Specialization exists ──────────────────────────────
            var specialization = await _unitOfWork.Specializations.GetByIdAsync(request.Dto.SpecializationId);
            if (specialization == null)
                throw new NotFoundException($"Specialization with ID '{request.Dto.SpecializationId}' not found.");

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
                var exists = await _unitOfWork.SpecializationCourses.ExistsAsync(
                    request.Dto.SpecializationId,
                    courseId);
                
                if (exists)
                {
                    errors.Add($"Course '{course.Code}' is already assigned to specialization '{specialization.Name}'.");
                }
            }

            if (errors.Any())
                throw new ValidationException(errors);

            // ─── 3. Create all mappings ──────────────────────────────────────────
            var mappings = new List<SpecializationCourse>();
            var createdCourses = new List<Course>();

            foreach (var course in existingCourses)
            {
                var mapping = new SpecializationCourse
                {
                    SpecializationId = request.Dto.SpecializationId,
                    CourseId = course.Id,
                    Role = request.Dto.Role,
                    CreatedAt = DateTime.UtcNow
                };
                mappings.Add(mapping);
                createdCourses.Add(course);
            }

            if (mappings.Any())
            {
                await _unitOfWork.SpecializationCourses.AddRangeAsync(mappings);
                await _unitOfWork.SaveChangesAsync();
            }

            // ─── 4. Get full details ──────────────────────────────────────────
            var createdMappings = await _unitOfWork.SpecializationCourses
                .GetBySpecializationIdWithDetailsAsync(request.Dto.SpecializationId);
            
            var result = createdMappings
                .Where(sc => request.Dto.CourseIds.Contains(sc.CourseId))
                .ToList();

            return _mapper.Map<IEnumerable<SpecializationCourseDto>>(result);
        }
    }
}