using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using University_Management_System.Application.Contracts;
using University_Management_System.Application.Dtos.RegistrationDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Enums;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Features.Registrations.Commands.BulkUpdateRegistrationGrades
{
    public class BulkUpdateRegistrationGradesCommandHandler
        : IRequestHandler<BulkUpdateRegistrationGradesCommand, IEnumerable<RegistrationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGpaCalculationService _gpaCalculationService;
        private readonly IMapper _mapper;

        public BulkUpdateRegistrationGradesCommandHandler(
            IGpaCalculationService gpaCalculationService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _gpaCalculationService = gpaCalculationService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RegistrationDto>> Handle(
            BulkUpdateRegistrationGradesCommand request,
            CancellationToken cancellationToken)
        {
            // ─── 1. Extract requested IDs ──────────────────────────────────────
            var requestedIds = request.Updates.Select(u => u.RegistrationId).ToList();

            // ─── 2. Get registrations by IDs ──────────────────────────────────
            var registrations = await _unitOfWork.Registrations.GetByIdsAsync(requestedIds);

            // ─── 3. Check for missing registrations ──────────────────────────
            if (registrations.Count() != requestedIds.Count)
            {
                var foundIds = registrations.Select(r => r.Id).ToHashSet();
                var missingIds = requestedIds.Where(id => !foundIds.Contains(id));

                throw new NotFoundException($"Registration(s) not found: {string.Join(", ", missingIds)}");
            }

            // ─── 4. Build update dictionary ────────────────────────────────────
            var updatesById = request.Updates.ToDictionary(u => u.RegistrationId);

            // ─── 5. Update registrations ──────────────────────────────────────
            foreach (var registration in registrations)
            {
                var update = updatesById[registration.Id];

                registration.Grade = update.Grade;
                registration.UpdatedAt = DateTime.UtcNow;
                registration.Progress = CourseProgress.Completed;

                // Auto-update progress based on grade
                if (update.Grade.HasValue)
                {
                    // If grade is F or below, not passed
                    if (update.Grade.Value == Grades.F || update.Grade.Value == Grades.D_Minus)
                    {
                        registration.IsPassed = false;
                    }

                    // If grade is C or higher, mark as completed
                    if (update.Grade.Value >= Grades.C && registration.Progress != CourseProgress.Completed)
                    {
                        registration.IsPassed = true;
                    }
                }
            }

            await _unitOfWork.SaveChangesAsync();

            // ─── 6. Cascade: Recalculate GPA for touched registrations ──────
            await _gpaCalculationService.RecalculateGpaForRegistrationsAsync(registrations);

            // ─── 7. Return DTOs using AutoMapper ──────────────────────────────────
            return _mapper.Map<IEnumerable<RegistrationDto>>(registrations);
        }
    }
}