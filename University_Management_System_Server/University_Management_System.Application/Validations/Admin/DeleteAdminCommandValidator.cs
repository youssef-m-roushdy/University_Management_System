using FluentValidation;
using University_Management_System.Application.Commands.Admins;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.Admin
{
    public class DeleteAdminCommandValidator : AbstractValidator<DeleteAdminCommand>
    {
        public DeleteAdminCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(ValidationMessages.Required);
        }
    }
}