using FluentValidation;
using University_Management_System.Application.Features.Registrations.Commands.UpdateRegistrationGrade;
using University_Management_System.Application.Validations.Common;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Validations.Registration
{
    public class UpdateRegistrationGradeCommandValidator : AbstractValidator<UpdateRegistrationGradeCommand>
    {
        public UpdateRegistrationGradeCommandValidator()
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