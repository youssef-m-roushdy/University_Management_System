using AutoMapper;
using MediatR;
using University_Management_System.Application.Commands.DepartmentCourses;
using University_Management_System.Application.Dtos.DepartmentCourseDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.DepartmentCourses
{
    public class UpdateDepartmentCourseCommandHandler : IRequestHandler<UpdateDepartmentCourseCommand, DepartmentCourseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateDepartmentCourseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DepartmentCourseDto> Handle(UpdateDepartmentCourseCommand request, CancellationToken cancellationToken)
        {
            // ─── 1. Validate Department exists ──────────────────────────────────
            var department = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId);
            if (department == null)
                throw new NotFoundException($"Department with ID '{request.DepartmentId}' not found.");

            // ─── 2. Validate Course exists ──────────────────────────────────
            var course = await _unitOfWork.Courses.GetByIdAsync(request.CourseId);
            if (course == null)
                throw new NotFoundException($"Course with ID '{request.CourseId}' not found.");

            // ─── 3. Check if mapping exists ──────────────────────────────────
            var exists = await _unitOfWork.DepartmentCourses.ExistsAsync(
                request.DepartmentId,
                request.CourseId);
            
            if (!exists)
                throw new NotFoundException($"Course '{course.Code}' is not assigned to department '{department.Name}'.");

            // ─── 4. Get the mapping ──────────────────────────────────────────
            var mappings = await _unitOfWork.DepartmentCourses
                .GetByDepartmentIdAsync(request.DepartmentId);
            
            var mapping = mappings.FirstOrDefault(dc => dc.CourseId == request.CourseId);
            
            if (mapping == null)
                throw new NotFoundException($"Mapping not found.");

            // ─── 5. Update the mapping ──────────────────────────────────────
            if (request.Dto.Role.HasValue)
                mapping.Role = request.Dto.Role.Value;

            mapping.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.DepartmentCourses.UpdateAsync(mapping);
            await _unitOfWork.SaveChangesAsync();

            // ─── 6. Get updated details ──────────────────────────────────────────
            var updated = await _unitOfWork.DepartmentCourses
                .GetByCourseIdWithDetailsAsync(request.CourseId);
            
            var result = updated.FirstOrDefault(dc => dc.DepartmentId == request.DepartmentId);

            return _mapper.Map<DepartmentCourseDto>(result);
        }
    }
}