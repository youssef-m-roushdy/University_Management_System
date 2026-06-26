using MediatR;
using University_Management_System.Application.Commands.SpecializationCourses;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.SpecializationCourses
{
    public class DeleteSpecializationCourseCommandHandler : IRequestHandler<DeleteSpecializationCourseCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSpecializationCourseCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteSpecializationCourseCommand request, CancellationToken cancellationToken)
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

            // ─── 4. Get and delete mapping ──────────────────────────────────
            var mappings = await _unitOfWork.SpecializationCourses
                .GetBySpecializationIdAsync(request.SpecializationId);
            
            var mapping = mappings.FirstOrDefault(sc => sc.CourseId == request.CourseId);
            
            if (mapping == null)
                throw new NotFoundException($"Mapping not found.");

            await _unitOfWork.SpecializationCourses.DeleteAsync(mapping);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}