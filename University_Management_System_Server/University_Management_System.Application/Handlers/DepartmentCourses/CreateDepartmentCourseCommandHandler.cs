using AutoMapper;
using MediatR;
using University_Management_System.Application.Commands.DepartmentCourses;
using University_Management_System.Application.Dtos.DepartmentCourseDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.DepartmentCourses
{
    public class CreateDepartmentCourseCommandHandler : IRequestHandler<CreateDepartmentCourseCommand, DepartmentCourseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateDepartmentCourseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DepartmentCourseDto> Handle(CreateDepartmentCourseCommand request, CancellationToken cancellationToken)
        {
            // ─── 1. Validate Department exists ──────────────────────────────────
            var department = await _unitOfWork.Departments.GetByIdAsync(request.Dto.DepartmentId);
            if (department == null)
                throw new NotFoundException($"Department with ID '{request.Dto.DepartmentId}' not found.");

            // ─── 2. Validate Course exists ──────────────────────────────────
            var course = await _unitOfWork.Courses.GetByIdAsync(request.Dto.CourseId);
            if (course == null)
                throw new NotFoundException($"Course with ID '{request.Dto.CourseId}' not found.");

            // ─── 3. Check if mapping already exists ──────────────────────────
            var exists = await _unitOfWork.DepartmentCourses.ExistsAsync(
                request.Dto.DepartmentId,
                request.Dto.CourseId);
            
            if (exists)
                throw new ValidationException(new List<string> {
                    $"Course '{course.Code}' is already assigned to department '{department.Name}'."
                });

            // ─── 4. Create mapping ─────────────────────────────────────────────
            var departmentCourse = new DepartmentCourse
            {
                DepartmentId = request.Dto.DepartmentId,
                CourseId = request.Dto.CourseId,
                Role = request.Dto.Role,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.DepartmentCourses.AddAsync(departmentCourse);
            await _unitOfWork.SaveChangesAsync();

            // ─── 5. Get full details ──────────────────────────────────────────
            var created = await _unitOfWork.DepartmentCourses
                .GetByCourseIdWithDetailsAsync(request.Dto.CourseId);
            
            var result = created.FirstOrDefault(dc => dc.DepartmentId == request.Dto.DepartmentId);

            return _mapper.Map<DepartmentCourseDto>(result);
        }
    }
}