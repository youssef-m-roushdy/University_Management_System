using MediatR;
using University_Management_System.Application.Commands.CoursePrerequisites;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.CoursePrerequisites
{
    public class DeleteCoursePrerequisiteCommandHandler : IRequestHandler<DeleteCoursePrerequisiteCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCoursePrerequisiteCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteCoursePrerequisiteCommand request, CancellationToken cancellationToken)
        {
            // ─── 1. Validate Course exists ──────────────────────────────────
            var course = await _unitOfWork.Courses.GetByIdAsync(request.CourseId);
            if (course == null)
                throw new NotFoundException($"Course with ID '{request.CourseId}' not found.");

            // ─── 2. Validate Prerequisite Course exists ─────────────────────
            var prerequisite = await _unitOfWork.Courses.GetByIdAsync(request.PrerequisiteCourseId);
            if (prerequisite == null)
                throw new NotFoundException($"Prerequisite course with ID '{request.PrerequisiteCourseId}' not found.");

            // ─── 3. Check if mapping exists using repository ─────────────────
            var exists = await _unitOfWork.CoursePrerequisites.ExistsAsync(
                request.CourseId,
                request.PrerequisiteCourseId);
            
            if (!exists)
                throw new NotFoundException($"Prerequisite relationship not found between '{course.Code}' and '{prerequisite.Code}'.");

            // ─── 4. Get the mapping ─────────────────────────────────────────
            var mappings = await _unitOfWork.CoursePrerequisites
                .GetByCourseIdAsync(request.CourseId);
            
            var mapping = mappings.FirstOrDefault(m => m.PrerequisiteCourseId == request.PrerequisiteCourseId);
            
            if (mapping == null)
                throw new NotFoundException($"Prerequisite relationship not found.");

            // ─── 5. Delete the mapping ──────────────────────────────────────
            await _unitOfWork.CoursePrerequisites.DeleteAsync(mapping);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}