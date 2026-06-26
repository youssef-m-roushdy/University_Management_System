using FluentValidation;
using University_Management_System.Application.Dtos.RegistrationDtos;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.Registration
{
    public class UpdateRegistrationDtoValidator : AbstractValidator<UpdateRegistrationDto>
    {
        public UpdateRegistrationDtoValidator()
        {
            RuleFor(x => x.Status)
                .IsInEnum().WithMessage(ValidationMessages.InvalidEnumValue)
                .When(x => x.Status.HasValue);

            RuleFor(x => x.Progress)
                .IsInEnum().WithMessage(ValidationMessages.InvalidEnumValue)
                .When(x => x.Progress.HasValue);

            RuleFor(x => x.Grade)
                .IsInEnum().WithMessage(ValidationMessages.InvalidEnumValue)
                .When(x => x.Grade.HasValue);

            RuleFor(x => x.Reason)
                .MaximumLength(500).WithMessage(ValidationMessages.MaxLength)
                .When(x => !string.IsNullOrEmpty(x.Reason));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(ValidationMessages.AtLeastOneField);
        }

        private static bool HaveAtLeastOneField(UpdateRegistrationDto dto)
        {
            return dto.Status.HasValue ||
                   dto.Progress.HasValue ||
                   dto.Grade.HasValue ||
                   !string.IsNullOrEmpty(dto.Reason);
        }
    }
}