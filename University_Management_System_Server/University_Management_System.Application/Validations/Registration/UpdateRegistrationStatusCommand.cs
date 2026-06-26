using FluentValidation;
using University_Management_System.Application.Commands.Registrations;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.Registration
{
    public class UpdateRegistrationStatusCommandValidator : AbstractValidator<UpdateRegistrationStatusCommand>
    {
        public UpdateRegistrationStatusCommandValidator()
        {
            RuleFor(x => x.RegistrationId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage(ValidationMessages.InvalidEnumValue);

            RuleFor(x => x.Reason)
                .MaximumLength(500).WithMessage(ValidationMessages.MaxLength)
                .When(x => !string.IsNullOrEmpty(x.Reason));
        }
    }
}