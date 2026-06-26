using AutoMapper;
using MediatR;
using University_Management_System.Application.Commands.SpecializationCourses;
using University_Management_System.Application.Dtos.SpecializationCourseDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.SpecializationCourses
{
    public class UpdateSpecializationCourseCommandHandler : IRequestHandler<UpdateSpecializationCourseCommand, SpecializationCourseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateSpecializationCourseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SpecializationCourseDto> Handle(UpdateSpecializationCourseCommand request, CancellationToken cancellationToken)
        {
            // ─── 1. Validate Specialization exists ──────────────────────────────
            var specialization = await _unitOfWork.Specializations.GetByIdAsync(request.SpecializationId);
            if (specialization == null)
                throw new NotFoundException($"Specialization with ID '{request.SpecializationId}' not found.");

            // ─── 2. Validate Course exists ──────────────────────────────────
            var course = await _unitOfWork.Courses.GetByIdAsync(request.CourseId);
            if (course == null)
                throw new NotFoundException($"Course with ID '{request.CourseId}' not found.");

            // ─── 3. Check if mapping exists ──────────────────────────────────
            var exists = await _unitOfWork.SpecializationCourses.ExistsAsync(
                request.SpecializationId,
                request.CourseId);
            
            if (!exists)
                throw new NotFoundException($"Course '{course.Code}' is not assigned to specialization '{specialization.Name}'.");

            // ─── 4. Get the mapping ──────────────────────────────────────────
            var mappings = await _unitOfWork.SpecializationCourses
                .GetBySpecializationIdAsync(request.SpecializationId);
            
            var mapping = mappings.FirstOrDefault(sc => sc.CourseId == request.CourseId);
            
            if (mapping == null)
                throw new NotFoundException($"Mapping not found.");

            // ─── 5. Update the mapping ──────────────────────────────────────
            if (request.Dto.Role.HasValue)
                mapping.Role = request.Dto.Role.Value;

            mapping.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SpecializationCourses.UpdateAsync(mapping);
            await _unitOfWork.SaveChangesAsync();

            // ─── 6. Get updated details ──────────────────────────────────────────
            var updated = await _unitOfWork.SpecializationCourses
                .GetByCourseIdWithDetailsAsync(request.CourseId);
            
            var result = updated.FirstOrDefault(sc => sc.SpecializationId == request.SpecializationId);

            return _mapper.Map<SpecializationCourseDto>(result);
        }
    }
}