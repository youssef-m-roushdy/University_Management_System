using MediatR;
using University_Management_System.Application.Commands.Courses;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Courses
{
    public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCourseCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
            // ─── 1. Check if Course exists ──────────────────────────────────
            var course = await _unitOfWork.Courses
                .GetCourseWithDetailsAsync(request.Id);
            
            if (course == null)
                throw new NotFoundException($"Course with ID '{request.Id}' not found.");

            // ─── 2. Check if course has registrations ──────────────────────
            var hasRegistrations = await _unitOfWork.Registrations
                .GetRegistrationCountByCourseAsync(request.Id) > 0;
            
            if (hasRegistrations)
                throw new ValidationException(new List<string> {
                    "Cannot delete course with existing registrations."
                });

            // ─── 3. Remove Prerequisite mappings ─────────────────────────────
            var prerequisites = await _unitOfWork.CoursePrerequisites
                .GetByCourseIdAsync(request.Id);
            
            if (prerequisites.Any())
                await _unitOfWork.CoursePrerequisites.DeleteRangeAsync(prerequisites);

            // ─── 4. Delete Course ─────────────────────────────────────────────
            await _unitOfWork.Courses.DeleteAsync(course);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}