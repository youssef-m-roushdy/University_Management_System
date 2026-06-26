using AutoMapper;
using MediatR;
using University_Management_System.Application.Commands.Registrations;
using University_Management_System.Application.Contracts;
using University_Management_System.Application.Dtos.RegistrationDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Enums;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Registrations
{
    public class UpdateRegistrationCommandHandler : IRequestHandler<UpdateRegistrationCommand, RegistrationDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGpaCalculationService _gpaCalculationService;
        private readonly IMapper _mapper;

        public UpdateRegistrationCommandHandler(
            IUnitOfWork unitOfWork,
            IGpaCalculationService gpaCalculationService,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _gpaCalculationService = gpaCalculationService;
            _mapper = mapper;
        }

        public async Task<RegistrationDto> Handle(
            UpdateRegistrationCommand request,
            CancellationToken cancellationToken)
        {
            // ─── 1. Get registration ──────────────────────────────────────────
            var registration = await _unitOfWork.Registrations
                .GetByIdAsync(request.Id);

            if (registration is null)
                throw new NotFoundException($"Registration with ID '{request.Id}' was not found.");

            // ─── 2. Verify ownership ──────────────────────────────────────────
            if (registration.StudentId != request.StudentId)
                throw new UnauthorizedAccessException("You can only update your own registrations.");

            // ─── 3. Track changes for GPA recalculation ──────────────────────
            var gradeChanged = false;

            // ─── 4. Update registration fields ────────────────────────────────
            if (request.Dto is not null)
            {
                // Update Status
                if (request.Dto.Status.HasValue)
                    registration.Status = request.Dto.Status.Value;

                // Update Reason
                if (!string.IsNullOrEmpty(request.Dto.Reason))
                    registration.Reason = request.Dto.Reason;

                // Update Grade
                if (request.Dto.Grade.HasValue)
                {
                    registration.Grade = request.Dto.Grade.Value;
                    gradeChanged = true;
                    registration.Progress = CourseProgress.Completed;

                    // Auto-update IsPassed based on grade
                    if (request.Dto.Grade.Value == Grades.F || request.Dto.Grade.Value == Grades.D_Minus)
                    {
                        registration.IsPassed = false;
                    }
                    else
                    {
                        registration.IsPassed = true;
                    }
                }
            }

            registration.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Registrations.UpdateAsync(registration);
            await _unitOfWork.SaveChangesAsync();

            // ─── 5. Recalculate GPA if grade was updated ─────────────────────
            if (gradeChanged)
            {
                await _gpaCalculationService.RecalculateGpaForRegistrationsAsync(
                    new[] { registration });
            }

            // ─── 6. Get updated registration with details ────────────────────
            var updatedRegistration = await _unitOfWork.Registrations
                .GetByIdAsync(request.Id);

            return _mapper.Map<RegistrationDto>(updatedRegistration);
        }
    }
}