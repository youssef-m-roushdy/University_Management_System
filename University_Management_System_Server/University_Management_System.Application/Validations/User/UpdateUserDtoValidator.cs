using FluentValidation;
using University_Management_System.Application.Dtos.UserDtos;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.User
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            // ─── Name Validation ──────────────────────────────────────────────
            RuleFor(x => x.Name)
                .ValidName().When(x => !string.IsNullOrEmpty(x.Name));

            // ─── Phone Number Validation ──────────────────────────────────────
            RuleFor(x => x.PhoneNumber)
                .ValidPhoneNumber().When(x => !string.IsNullOrEmpty(x.PhoneNumber));

            // ─── Address Validation ───────────────────────────────────────────
            RuleFor(x => x.Address)
                .MaximumLength(200).WithMessage(ValidationMessages.MaxLength)
                .When(x => !string.IsNullOrEmpty(x.Address));

            // ─── Gender Validation ────────────────────────────────────────────
            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage(ValidationMessages.InvalidEnumValue)
                .When(x => x.Gender.HasValue);

            // ─── At least one field must be provided ─────────────────────────
            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage("At least one field must be provided for update");
        }

        private static bool HaveAtLeastOneField(UpdateUserDto dto)
        {
            return !string.IsNullOrEmpty(dto.Name) ||
                   !string.IsNullOrEmpty(dto.PhoneNumber) ||
                   !string.IsNullOrEmpty(dto.Address) ||
                   dto.Gender.HasValue;
        }
    }
}