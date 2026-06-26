using FluentValidation;
using University_Management_System.Application.Commands.Registrations;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.Registration
{
    public class DeleteRegistrationCommandValidator : AbstractValidator<DeleteRegistrationCommand>
    {
        public DeleteRegistrationCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);
        }
    }
}