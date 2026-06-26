using AutoMapper;
using MediatR;
using University_Management_System.Application.Commands.CoursePrerequisites;
using University_Management_System.Application.Dtos.CourseDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.CoursePrerequisites
{
    public class CreateCoursePrerequisiteBulkCommandHandler : IRequestHandler<CreateCoursePrerequisiteBulkCommand, IEnumerable<CourseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateCoursePrerequisiteBulkCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CourseDto>> Handle(
            CreateCoursePrerequisiteBulkCommand request,
            CancellationToken cancellationToken)
        {
            // ─── 1. Validate Course exists ──────────────────────────────────
            var course = await _unitOfWork.Courses.GetByIdAsync(request.Dto.CourseId);
            if (course == null)
                throw new NotFoundException($"Course with ID '{request.Dto.CourseId}' not found.");

            // ─── 2. Validate all prerequisite courses exist ────────────────
            var prerequisiteIds = request.Dto.PrerequisiteCourseIds.Distinct().ToList();
            var existingPrereqs = new List<Course>();
            
            foreach (var prereqId in prerequisiteIds)
            {
                var prereq = await _unitOfWork.Courses.GetByIdAsync(prereqId);
                if (prereq == null)
                    throw new NotFoundException($"Prerequisite course with ID '{prereqId}' not found.");
                existingPrereqs.Add(prereq);
            }

            // ─── 3. Validate no self-reference ──────────────────────────────
            if (prerequisiteIds.Contains(request.Dto.CourseId))
                throw new ValidationException(new List<string> {
                    $"Course '{course.Code}' cannot be a prerequisite of itself."
                });

            // ─── 4. Check existing relationships ─────────────────────────────
            var errors = new List<string>();
            var mappings = new List<CoursePrerequisite>();
            var createdCourses = new List<Course>();

            foreach (var prereqId in prerequisiteIds)
            {
                // Get the prerequisite course for this iteration
                var prereq = existingPrereqs.First(p => p.Id == prereqId);

                // Check if relationship already exists
                var exists = await _unitOfWork.CoursePrerequisites.ExistsAsync(
                    request.Dto.CourseId,
                    prereqId);
                
                if (exists)
                {
                    errors.Add($"Prerequisite relationship already exists between '{course.Code}' and '{prereq.Code}'.");
                    continue;
                }

                // Check for circular dependency
                var hasCircular = await HasCircularDependencyAsync(request.Dto.CourseId, prereqId);
                if (hasCircular)
                {
                    errors.Add($"Circular dependency detected: '{prereq.Code}' depends on '{course.Code}'.");
                    continue;
                }

                var mapping = new CoursePrerequisite
                {
                    CourseId = request.Dto.CourseId,
                    PrerequisiteCourseId = prereqId,
                    CreatedAt = DateTime.UtcNow
                };
                mappings.Add(mapping);
                createdCourses.Add(prereq);
            }

            if (errors.Any())
                throw new ValidationException(errors);

            // ─── 5. Create all mappings ──────────────────────────────────────
            if (mappings.Any())
            {
                await _unitOfWork.CoursePrerequisites.AddRangeAsync(mappings);
                await _unitOfWork.SaveChangesAsync();
            }

            // ─── 6. Return all prerequisite courses that were added ──────────
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