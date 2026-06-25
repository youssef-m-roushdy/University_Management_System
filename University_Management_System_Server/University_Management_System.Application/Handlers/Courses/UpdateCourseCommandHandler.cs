using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using University_Management_System.Application.Commands.Courses;
using University_Management_System.Application.Dtos.CourseDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Courses
{
    public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, CourseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateCourseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CourseDto> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            // ─── 1. Check if Course exists ──────────────────────────────────
            var course = await _unitOfWork.Courses
                .GetCourseWithDetailsAsync(request.Id);
            
            if (course == null)
                throw new NotFoundException($"Course with ID '{request.Id}' not found.");

            // ─── 2. Validate Department ──────────────────────────────────────
            if (request.Dto.DepartmentId.HasValue)
            {
                var department = await _unitOfWork.Departments
                    .GetByIdAsync(request.Dto.DepartmentId.Value);
                
                if (department == null)
                    throw new NotFoundException($"Department with ID '{request.Dto.DepartmentId}' not found.");
            }

            // ─── 3. Validate Course Code ─────────────────────────────────────
            if (!string.IsNullOrEmpty(request.Dto.Code) && request.Dto.Code != course.Code)
            {
                var codeExists = await _unitOfWork.Courses
                    .CourseCodeExistsAsync(request.Dto.Code);
                
                if (codeExists)
                    throw new ValidationException(new List<string> {
                        $"Course with code '{request.Dto.Code}' already exists."
                    });
            }

            // ─── 5. Update Course ─────────────────────────────────────────────
            _mapper.Map(request.Dto, course);
            course.UpdatedAt = DateTime.UtcNow;
              

            await _unitOfWork.Courses.UpdateAsync(course);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CourseDto>(course);
        }

        private async Task<bool> HasCircularDependencyAsync(int courseId, int prerequisiteId)
        {
            // Check if the prerequisite course depends on the current course
            var dependencies = await _unitOfWork.Courses
                .GetDependenciesAsync(prerequisiteId);
            
            return dependencies.Any(d => d.Id == courseId);
        }
    }
}