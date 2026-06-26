using AutoMapper;
using MediatR;
using University_Management_System.Application.Commands.SpecializationCourses;
using University_Management_System.Application.Dtos.SpecializationCourseDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.SpecializationCourses
{
    public class CreateSpecializationCourseCommandHandler : IRequestHandler<CreateSpecializationCourseCommand, SpecializationCourseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateSpecializationCourseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SpecializationCourseDto> Handle(CreateSpecializationCourseCommand request, CancellationToken cancellationToken)
        {
            // ─── 1. Validate Specialization exists ──────────────────────────────
            var specialization = await _unitOfWork.Specializations.GetByIdAsync(request.Dto.SpecializationId);
            if (specialization == null)
                throw new NotFoundException($"Specialization with ID '{request.Dto.SpecializationId}' not found.");

            // ─── 2. Validate Course exists ──────────────────────────────────
            var course = await _unitOfWork.Courses.GetByIdAsync(request.Dto.CourseId);
            if (course == null)
                throw new NotFoundException($"Course with ID '{request.Dto.CourseId}' not found.");

            // ─── 3. Check if mapping already exists ──────────────────────────
            var exists = await _unitOfWork.SpecializationCourses.ExistsAsync(
                request.Dto.SpecializationId,
                request.Dto.CourseId);
            
            if (exists)
                throw new ValidationException(new List<string> {
                    $"Course '{course.Code}' is already assigned to specialization '{specialization.Name}'."
                });

            // ─── 4. Create mapping ─────────────────────────────────────────────
            var specializationCourse = new SpecializationCourse
            {
                SpecializationId = request.Dto.SpecializationId,
                CourseId = request.Dto.CourseId,
                Role = request.Dto.Role,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.SpecializationCourses.AddAsync(specializationCourse);
            await _unitOfWork.SaveChangesAsync();

            // ─── 5. Get full details ──────────────────────────────────────────
            var created = await _unitOfWork.SpecializationCourses
                .GetByCourseIdWithDetailsAsync(request.Dto.CourseId);
            
            var result = created.FirstOrDefault(sc => sc.SpecializationId == request.Dto.SpecializationId);

            return _mapper.Map<SpecializationCourseDto>(result);
        }
    }
}