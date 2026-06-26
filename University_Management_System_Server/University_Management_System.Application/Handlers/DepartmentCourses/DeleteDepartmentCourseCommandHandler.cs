using MediatR;
using University_Management_System.Application.Commands.DepartmentCourses;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.DepartmentCourses
{
    public class DeleteDepartmentCourseCommandHandler : IRequestHandler<DeleteDepartmentCourseCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDepartmentCourseCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteDepartmentCourseCommand request, CancellationToken cancellationToken)
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

            // ─── 4. Get and delete mapping ──────────────────────────────────
            var mappings = await _unitOfWork.DepartmentCourses
                .GetByDepartmentIdAsync(request.DepartmentId);
            
            var mapping = mappings.FirstOrDefault(dc => dc.CourseId == request.CourseId);
            
            if (mapping == null)
                throw new NotFoundException($"Mapping not found.");

            await _unitOfWork.DepartmentCourses.DeleteAsync(mapping);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}