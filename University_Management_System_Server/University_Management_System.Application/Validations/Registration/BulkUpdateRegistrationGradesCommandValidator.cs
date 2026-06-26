using FluentValidation;
using University_Management_System.Application.Dtos.RegistrationDtos;
using University_Management_System.Application.Features.Registrations.Commands.BulkUpdateRegistrationGrades;
using University_Management_System.Application.Validations.Common;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Validations.Registration
{
    public class BulkUpdateRegistrationGradesCommandValidator : AbstractValidator<BulkUpdateRegistrationGradesCommand>
    {
        public BulkUpdateRegistrationGradesCommandValidator()
        {
            // ─── Updates List ──────────────────────────────────────────────────
            RuleFor(x => x.Updates)
                .NotNull().WithMessage("At least one registration update is required")
                .Must(list => list != null && list.Any()).WithMessage("At least one registration update is required");

            // ─── Validate each item in the list ─────────────────────────────
            RuleForEach(x => x.Updates).SetValidator(new UpdateRegistrationGradeDtoValidator());

            // ─── Ensure no duplicate registration IDs ────────────────────────
            RuleFor(x => x.Updates)
                .Must(list => list != null && list.Select(u => u.RegistrationId).Distinct().Count() == list.Count)
                .WithMessage("Duplicate registration IDs are not allowed")
                .When(x => x.Updates != null);

        }
    }

    // ─── Nested Validator for individual updates ──────────────────────────
    public class UpdateRegistrationGradeDtoValidator : AbstractValidator<UpdateRegistrationGradeDto>
    {
        public UpdateRegistrationGradeDtoValidator()
        {
            // ─── Registration ID ──────────────────────────────────────────────
            RuleFor(x => x.RegistrationId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);

            // ─── Grade ────────────────────────────────────────────────────────
            RuleFor(x => x.Grade)
                .IsInEnum().WithMessage(ValidationMessages.InvalidEnumValue)
                .When(x => x.Grade.HasValue);
        }
    }
}