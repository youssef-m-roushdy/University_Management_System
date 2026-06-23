using FluentValidation;
using University_Management_System.Application.Dtos.SemesterDtos;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.Semesters
{
    public class CreateSemesterDtoValidator : AbstractValidator<CreateSemesterDto>
    {
        public CreateSemesterDtoValidator()
        {
            // ─── Title Validation ────────────────────────────────────────────────
            RuleFor(x => x.Title)
                .IsInEnum().WithMessage(ValidationMessages.InvalidEnumValue);

            // ─── StartDate Validation ────────────────────────────────────────────
            RuleFor(x => x.StartDate)
                .NotEqual(default(DateTime)).WithMessage(ValidationMessages.InvalidDate);

            // ─── EndDate Validation ──────────────────────────────────────────────
            RuleFor(x => x.EndDate)
                .NotEqual(default(DateTime)).WithMessage(ValidationMessages.InvalidDate)
                .GreaterThan(x => x.StartDate).WithMessage(ValidationMessages.DateRangeMinMax);
        }
    }
}