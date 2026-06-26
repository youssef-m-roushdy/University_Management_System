using AutoMapper;
using MediatR;
using University_Management_System.Application.Commands.CoursePrerequisites;
using University_Management_System.Application.Dtos.CourseDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.CoursePrerequisites
{
    public class CreateCourseDependencyCommandHandler : IRequestHandler<CreateCourseDependencyCommand, CourseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateCourseDependencyCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CourseDto> Handle(CreateCourseDependencyCommand request, CancellationToken cancellationToken)
        {
            // ─── 1. Validate Prerequisite Course exists ─────────────────────
            var prerequisite = await _unitOfWork.Courses.GetByIdAsync(request.Dto.PrerequisiteCourseId);
            if (prerequisite == null)
                throw new NotFoundException($"Prerequisite course with ID '{request.Dto.PrerequisiteCourseId}' not found.");

            // ─── 2. Validate Dependent Course exists ────────────────────────
            var dependent = await _unitOfWork.Courses.GetByIdAsync(request.Dto.CourseId);
            if (dependent == null)
                throw new NotFoundException($"Dependent course with ID '{request.Dto.CourseId}' not found.");

            // ─── 3. Prevent self-reference ──────────────────────────────────
            if (request.Dto.CourseId == request.Dto.PrerequisiteCourseId)
                throw new ValidationException(new List<string> {
                    "A course cannot depend on itself."
                });

            // ─── 4. Check if relationship already exists ────────────────────
            var exists = await _unitOfWork.CoursePrerequisites.ExistsAsync(
                request.Dto.CourseId,
                request.Dto.PrerequisiteCourseId);
            
            if (exists)
                throw new ValidationException(new List<string> {
                    $"Dependency already exists: '{dependent.Code}' already depends on '{prerequisite.Code}'."
                });

            // ─── 5. Check for circular dependency ────────────────────────────
            var hasCircular = await HasCircularDependencyAsync(
                request.Dto.CourseId,
                request.Dto.PrerequisiteCourseId);
            
            if (hasCircular)
                throw new ValidationException(new List<string> {
                    $"Circular dependency detected: '{prerequisite.Code}' depends on '{dependent.Code}'."
                });

            // ─── 6. Create Dependency Mapping ──────────────────────────────
            var mapping = new CoursePrerequisite
            {
                CourseId = request.Dto.CourseId,
                PrerequisiteCourseId = request.Dto.PrerequisiteCourseId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.CoursePrerequisites.AddAsync(mapping);
            await _unitOfWork.SaveChangesAsync();

            // ─── 7. Return updated dependent course ──────────────────────────
            var updatedCourse = await _unitOfWork.Courses
                .GetCourseWithDetailsAsync(request.Dto.CourseId);

            return _mapper.Map<CourseDto>(updatedCourse);
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