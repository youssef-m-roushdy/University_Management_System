using FluentValidation;
using University_Management_System.Application.Dtos.StudentStudyYearDtos;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.StudentStudyYear
{
    public class UpdateStudentStudyYearDtoValidator : AbstractValidator<UpdateStudentStudyYearDto>
    {
        public UpdateStudentStudyYearDtoValidator()
        {
            // ─── Level ──────────────────────────────────────────────────────
            RuleFor(x => x.Level)
                .IsInEnum()
                .WithMessage(ValidationMessages.InvalidEnumValue)
                .When(x => x.Level.HasValue);

            // ─── Validation: IsActive──
            RuleFor(x => x.IsActive)
                .NotNull()
                .WithMessage(ValidationMessages.Required)
                .When(x => x.IsActive.HasValue);

            // ─── At least one field must be provided ──────────────────────
            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(ValidationMessages.AtLeastOneField);
        }

        private static bool HaveAtLeastOneField(UpdateStudentStudyYearDto dto)
        {
            return dto.Level.HasValue ||
                   dto.IsActive.HasValue;
        }
    }
}