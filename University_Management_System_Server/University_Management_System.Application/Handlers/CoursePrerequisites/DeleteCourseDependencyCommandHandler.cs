using MediatR;
using University_Management_System.Application.Commands.CoursePrerequisites;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.CoursePrerequisites
{
    public class DeleteCourseDependencyCommandHandler : IRequestHandler<DeleteCourseDependencyCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCourseDependencyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteCourseDependencyCommand request, CancellationToken cancellationToken)
        {
            // ─── 1. Validate Dependent Course exists ──────────────────────
            var dependent = await _unitOfWork.Courses.GetByIdAsync(request.CourseId);
            if (dependent == null)
                throw new NotFoundException($"Dependent course with ID '{request.CourseId}' not found.");

            // ─── 2. Validate Prerequisite Course exists ─────────────────────
            var prerequisite = await _unitOfWork.Courses.GetByIdAsync(request.DependencyCourseId);
            if (prerequisite == null)
                throw new NotFoundException($"Prerequisite course with ID '{request.DependencyCourseId}' not found.");

            // ─── 3. Check if mapping exists using repository ─────────────────
            var exists = await _unitOfWork.CoursePrerequisites.ExistsAsync(
                request.CourseId,
                request.DependencyCourseId);
            
            if (!exists)
                throw new NotFoundException($"Dependency not found: '{dependent.Code}' does not depend on '{prerequisite.Code}'.");

            // ─── 4. Get the mapping ─────────────────────────────────────────
            var mappings = await _unitOfWork.CoursePrerequisites
                .GetByCourseIdAsync(request.CourseId);
            
            var mapping = mappings.FirstOrDefault(m => m.PrerequisiteCourseId == request.DependencyCourseId);
            
            if (mapping == null)
                throw new NotFoundException($"Dependency relationship not found.");

            // ─── 5. Delete the mapping ──────────────────────────────────────
            await _unitOfWork.CoursePrerequisites.DeleteAsync(mapping);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}