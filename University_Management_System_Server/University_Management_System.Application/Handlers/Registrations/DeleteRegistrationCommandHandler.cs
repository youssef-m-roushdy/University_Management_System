using MediatR;
using University_Management_System.Application.Commands.Registrations;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Enums;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Registrations
{
    public class DeleteRegistrationCommandHandler : IRequestHandler<DeleteRegistrationCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteRegistrationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteRegistrationCommand request, CancellationToken cancellationToken)
        {
            // ─── 1. Get registration ──────────────────────────────────────────
            var registration = await _unitOfWork.Registrations
                .GetByIdAsync(request.Id);

            if (registration == null)
                throw new NotFoundException($"Registration with ID '{request.Id}' not found.");

            // ─── 2. Verify ownership ──────────────────────────────────────────
            if (registration.StudentId != request.StudentId)
                throw new UnauthorizedAccessException("You can only delete your own registrations.");

            // ─── 3. Check if registration can be deleted ──────────────────────
            if (registration.Progress == CourseProgress.Completed)
                throw new ValidationException(new List<string> {
                    "Cannot delete a completed registration."
                });

            // ─── 4. Get student and course for credit management ─────────────
            var student = await _unitOfWork.Students.GetByIdAsync(registration.StudentId);
            if (student == null)
                throw new NotFoundException($"Student with ID '{registration.StudentId}' not found.");

            var course = await _unitOfWork.Courses.GetByIdAsync(registration.CourseId);
            if (course == null)
                throw new NotFoundException($"Course with ID '{registration.CourseId}' not found.");

            // ─── 5. ✅ INCREASE student's allowed credits ──────────────────
            student.AllowedCredits += course.Credits;
            student.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.Students.UpdateAsync(student);

            // ─── 6. Delete registration ──────────────────────────────────────
            await _unitOfWork.Registrations.DeleteAsync(registration);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}