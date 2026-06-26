using FluentValidation;
using University_Management_System.Application.Dtos.SpecializationDtos;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.Specialization
{
    public class UpdateSpecializationDtoValidator : AbstractValidator<UpdateSpecializationDto>
    {
        public UpdateSpecializationDtoValidator()
        {
            // ─── Name ──────────────────────────────────────────────────────────
            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage(ValidationMessages.MaxLength)
                .Matches(@"^[a-zA-Z\s\-]+$")
                .WithMessage("Specialization name can only contain letters, spaces, and hyphens")
                .When(x => !string.IsNullOrEmpty(x.Name));

            // ─── Description ───────────────────────────────────────────────────
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage(ValidationMessages.MaxLength)
                .When(x => !string.IsNullOrEmpty(x.Description));

            // ─── Department ──────────────────────────────────────────────────
            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId)
                .When(x => x.DepartmentId.HasValue);

            // ─── At least one field must be provided ─────────────────────────
            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(ValidationMessages.AtLeastOneField);
        }

        private static bool HaveAtLeastOneField(UpdateSpecializationDto dto)
        {
            return !string.IsNullOrEmpty(dto.Name) ||
                   !string.IsNullOrEmpty(dto.Description) ||
                   dto.DepartmentId.HasValue;
        }
    }
}