using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using University_Management_System.Application.Contracts;
using University_Management_System.Application.Dtos.RegistrationDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Enums;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Features.Registrations.Commands.UpdateRegistrationGrade
{
    public class UpdateRegistrationGradeCommandHandler
        : IRequestHandler<UpdateRegistrationGradeCommand, RegistrationDto>
    {
        private readonly IGpaCalculationService _gpaCalculationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateRegistrationGradeCommandHandler(
            IGpaCalculationService gpaCalculationService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _gpaCalculationService = gpaCalculationService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RegistrationDto> Handle(
            UpdateRegistrationGradeCommand request,
            CancellationToken cancellationToken)
        {
            // ─── 1. Get registration ──────────────────────────────────────────
            var registration = await _unitOfWork.Registrations.GetByIdAsync(request.RegistrationId);

            if (registration is null)
            {
                throw new NotFoundException($"Registration with id {request.RegistrationId} was not found.");
            }

            // ─── 2. Update grade and IsPassed ──────────────────────────────────
            registration.Grade = request.Grade;
            registration.UpdatedAt = System.DateTime.UtcNow;
            registration.Progress = CourseProgress.Completed;

            // ─── 3. Auto-update progress based on grade ──────────────────────
            if (request.Grade.HasValue)
            {
                // If grade is F or below, not passed
                if (request.Grade.Value == Grades.F || request.Grade.Value == Grades.D_Minus)
                {
                    registration.IsPassed = false;
                }
                
                // If grade is C or higher, mark as completed
                if (request.Grade.Value >= Grades.C && registration.Progress != CourseProgress.Completed)
                {
                    registration.IsPassed = false;
                }
            }

            await _unitOfWork.SaveChangesAsync();

            // ─── 4. Cascade: Recalculate SemesterGPA for this semester ──────
            await _gpaCalculationService.RecalculateGpaForRegistrationsAsync(new[] { registration });

            // ─── 5. Get updated registration and return DTO using AutoMapper ──
            var updatedRegistration = await _unitOfWork.Registrations
                .GetByIdAsync(request.RegistrationId);

            return _mapper.Map<RegistrationDto>(updatedRegistration);
        }
    }
}