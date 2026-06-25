using AutoMapper;
using MediatR;
using University_Management_System.Application.Commands.Courses;
using University_Management_System.Application.Dtos.CourseDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Courses
{
    public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, CourseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateCourseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CourseDto> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            // ─── 1. Validate Department ──────────────────────────────────────
            var department = await _unitOfWork.Departments
                .GetByIdAsync(request.Dto.DepartmentId);
            
            if (department == null)
                throw new NotFoundException($"Department with ID '{request.Dto.DepartmentId}' not found.");

            // ─── 2. Validate Course Code ─────────────────────────────────────
            var codeExists = await _unitOfWork.Courses
                .CourseCodeExistsAsync(request.Dto.Code);
            
            if (codeExists)
                throw new ValidationException(new List<string> {
                    $"Course with code '{request.Dto.Code}' already exists."
                });

            // ─── 4. Create Course ─────────────────────────────────────────────
            var course = _mapper.Map<Course>(request.Dto);
            course.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Courses.AddAsync(course);
            await _unitOfWork.SaveChangesAsync();

            // ─── 6. Return mapped DTO ─────────────────────────────────────────
            return _mapper.Map<CourseDto>(course);
        }
    }
}