using AutoMapper;
using MediatR;
using University_Management_System.Application.Commands.CoursePrerequisites;
using University_Management_System.Application.Dtos.CourseDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.CoursePrerequisites
{
    public class CreateCourseDependencyBulkCommandHandler : IRequestHandler<CreateCourseDependencyBulkCommand, IEnumerable<CourseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateCourseDependencyBulkCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CourseDto>> Handle(
            CreateCourseDependencyBulkCommand request,
            CancellationToken cancellationToken)
        {
            // ─── 1. Validate Prerequisite Course exists ─────────────────────
            var prerequisite = await _unitOfWork.Courses.GetByIdAsync(request.Dto.PrerequisiteCourseId);
            if (prerequisite == null)
                throw new NotFoundException($"Prerequisite course with ID '{request.Dto.PrerequisiteCourseId}' not found.");

            // ─── 2. Validate all dependent courses exist ──────────────────
            var dependentIds = request.Dto.DependentCourseIds.Distinct().ToList();
            var existingDependents = new List<Course>();
            
            foreach (var depId in dependentIds)
            {
                var dependent = await _unitOfWork.Courses.GetByIdAsync(depId);
                if (dependent == null)
                    throw new NotFoundException($"Dependent course with ID '{depId}' not found.");
                existingDependents.Add(dependent);
            }

            // ─── 3. Validate no self-reference ──────────────────────────────
            if (dependentIds.Contains(request.Dto.PrerequisiteCourseId))
                throw new ValidationException(new List<string> {
                    $"Course '{prerequisite.Code}' cannot depend on itself."
                });

            // ─── 4. Check existing relationships ─────────────────────────────
            var errors = new List<string>();
            var mappings = new List<CoursePrerequisite>();
            var createdCourses = new List<Course>();

            foreach (var depId in dependentIds)
            {
                // Get the dependent course for this iteration
                var dependent = existingDependents.First(d => d.Id == depId);

                // Check if relationship already exists
                var exists = await _unitOfWork.CoursePrerequisites.ExistsAsync(
                    depId,
                    request.Dto.PrerequisiteCourseId);
                
                if (exists)
                {
                    errors.Add($"Dependency already exists: '{dependent.Code}' already depends on '{prerequisite.Code}'.");
                    continue;
                }

                // Check for circular dependency
                var hasCircular = await HasCircularDependencyAsync(depId, request.Dto.PrerequisiteCourseId);
                if (hasCircular)
                {
                    errors.Add($"Circular dependency detected: '{prerequisite.Code}' depends on '{dependent.Code}'.");
                    continue;
                }

                var mapping = new CoursePrerequisite
                {
                    CourseId = depId,
                    PrerequisiteCourseId = request.Dto.PrerequisiteCourseId,
                    CreatedAt = DateTime.UtcNow
                };
                mappings.Add(mapping);
                createdCourses.Add(dependent);
            }

            if (errors.Any())
                throw new ValidationException(errors);

            // ─── 5. Create all mappings ──────────────────────────────────────
            if (mappings.Any())
            {
                await _unitOfWork.CoursePrerequisites.AddRangeAsync(mappings);
                await _unitOfWork.SaveChangesAsync();
            }

            // ─── 6. Return all dependent courses that were added ─────────────
            return _mapper.Map<IEnumerable<CourseDto>>(createdCourses);
        }

        private async Task<bool> HasCircularDependencyAsync(int courseId, int prerequisiteId)
        {
            var dependencies = await _unitOfWork.CoursePrerequisites
                .GetByPrerequisiteCourseIdAsync(prerequisiteId);
            
            var dependentIds = dependencies.Select(d => d.CourseId).ToList();
            
            if (dependentIds.Contains(courseId))
                return true;

            foreach (var depId in dependentIds)
            {
                var hasCircular = await HasCircularDependencyAsync(courseId, depId);
                if (hasCircular)
                    return true;
            }

            return false;
        }
    }
}