using AutoMapper;
using MediatR;
using University_Management_System.Application.Commands.Registrations;
using University_Management_System.Application.Dtos.RegistrationDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Enums;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Registrations
{
    public class UpdateRegistrationStatusCommandHandler : IRequestHandler<UpdateRegistrationStatusCommand, RegistrationDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateRegistrationStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RegistrationDto> Handle(UpdateRegistrationStatusCommand request, CancellationToken cancellationToken)
        {
            // ─── 1. Get registration ──────────────────────────────────────────
            var registration = await _unitOfWork.Registrations
                .GetByIdAsync(request.RegistrationId);

            if (registration == null)
                throw new NotFoundException($"Registration with ID '{request.RegistrationId}' not found.");

            // ─── 2. Verify ownership ──────────────────────────────────────────
            if (registration.StudentId != request.StudentId)
                throw new UnauthorizedAccessException("You can only update your own registrations.");

            // ─── 3. Get student and course ──────────────────────────────────────
            var student = await _unitOfWork.Students.GetByIdAsync(registration.StudentId);
            if (student == null)
                throw new NotFoundException($"Student with ID '{registration.StudentId}' not found.");

            var course = await _unitOfWork.Courses.GetByIdAsync(registration.CourseId);
            if (course == null)
                throw new NotFoundException($"Course with ID '{registration.CourseId}' not found.");

            var oldStatus = registration.Status;
            var newStatus = request.Status;

            // ─── 4. Validate status transition ──────────────────────────────────
            var isValidTransition = IsValidStatusTransition(oldStatus, newStatus);
            if (!isValidTransition)
                throw new ValidationException(new List<string> {
                    $"Cannot transition from '{oldStatus}' to '{newStatus}'. Invalid status change."
                });

            // ─── 5. Handle status transitions and credit management ──────────

            // ─── 5a. If Approved: Update progress (credits already deducted) ──
            if (newStatus == RegistrationStatus.Approved 
                && oldStatus != RegistrationStatus.Approved)
            {
                // Credits are already deducted on creation (Pending status)
                // No need to deduct again
                registration.Progress = CourseProgress.InProgress;
            }

            // ─── 5b. If Rejected: Return credits ────────────────────────────
            if (newStatus == RegistrationStatus.Rejected 
                && (oldStatus == RegistrationStatus.Pending 
                    || oldStatus == RegistrationStatus.Approved))
            {
                // Return credits to student
                student.AllowedCredits += course.Credits;
                student.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Students.UpdateAsync(student);

                // Reset progress and grade
                registration.Progress = CourseProgress.NotStarted;
                registration.Grade = null;
                registration.IsPassed = false;
            }

            // ─── 5c. If Suspended: Return credits (if approved) ──────────────
            if (newStatus == RegistrationStatus.Suspended 
                && oldStatus == RegistrationStatus.Approved)
            {
                // Return credits to student
                student.AllowedCredits += course.Credits;
                student.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Students.UpdateAsync(student);

                // Reset progress
                registration.Progress = CourseProgress.NotStarted;
            }

            // ─── 5d. If Suspended → Approved: Deduct credits again ──────────
            if (newStatus == RegistrationStatus.Approved 
                && oldStatus == RegistrationStatus.Suspended)
            {
                // Check if student has enough credits
                if (student.AllowedCredits < course.Credits)
                    throw new ValidationException(new List<string> {
                        $"Student does not have enough allowed credits. Available: {student.AllowedCredits}, Required: {course.Credits}."
                    });

                // Deduct credits
                student.AllowedCredits -= course.Credits;
                student.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Students.UpdateAsync(student);

                // Update progress
                registration.Progress = CourseProgress.InProgress;
            }

            // ─── 6. Update status ──────────────────────────────────────────────
            registration.Status = newStatus;
            if (!string.IsNullOrEmpty(request.Reason))
                registration.Reason = request.Reason;
            registration.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Registrations.UpdateAsync(registration);
            await _unitOfWork.SaveChangesAsync();

            // ─── 7. Get updated registration ──────────────────────────────────
            var updatedRegistration = await _unitOfWork.Registrations
                .GetByIdAsync(request.RegistrationId);

            return _mapper.Map<RegistrationDto>(updatedRegistration);
        }

        private bool IsValidStatusTransition(RegistrationStatus oldStatus, RegistrationStatus newStatus)
        {
            // Define allowed transitions
            var allowedTransitions = new Dictionary<RegistrationStatus, List<RegistrationStatus>>()
            {
                // Pending can go to Approved, Rejected, or Suspended
                { RegistrationStatus.Pending, new List<RegistrationStatus> 
                    { RegistrationStatus.Approved, RegistrationStatus.Rejected, RegistrationStatus.Suspended } },
                
                // Approved can go to Suspended or Rejected
                { RegistrationStatus.Approved, new List<RegistrationStatus> 
                    { RegistrationStatus.Suspended, RegistrationStatus.Rejected } },
                
                // Suspended can go to Approved or Rejected
                { RegistrationStatus.Suspended, new List<RegistrationStatus> 
                    { RegistrationStatus.Approved, RegistrationStatus.Rejected } },
                
                // Rejected is final - cannot change
                { RegistrationStatus.Rejected, new List<RegistrationStatus>() }
            };

            if (!allowedTransitions.ContainsKey(oldStatus))
                return true;

            return allowedTransitions[oldStatus].Contains(newStatus);
        }
    }
}