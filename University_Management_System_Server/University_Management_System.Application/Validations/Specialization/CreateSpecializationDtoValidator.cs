using FluentValidation;
using University_Management_System.Application.Dtos.SpecializationDtos;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.Specialization
{
    public class CreateSpecializationDtoValidator : AbstractValidator<CreateSpecializationDto>
    {
        public CreateSpecializationDtoValidator()
        {
            // ─── Name ──────────────────────────────────────────────────────────
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(ValidationMessages.Required)
                .MaximumLength(100).WithMessage(ValidationMessages.MaxLength)
                .Matches(@"^[a-zA-Z\s\-]+$")
                .WithMessage("Specialization name can only contain letters, spaces, and hyphens");

            // ─── Description ───────────────────────────────────────────────────
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage(ValidationMessages.MaxLength)
                .When(x => !string.IsNullOrEmpty(x.Description));

            // ─── Department ──────────────────────────────────────────────────
            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);
        }
    }
}