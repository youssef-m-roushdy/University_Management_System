using FluentValidation;
using University_Management_System.Application.Dtos.AuthDtos;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.Auth
{
    public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordDtoValidator()
        {
            RuleFor(x => x.Email)
                .ValidEmail();

            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Reset token is required");

            RuleFor(x => x.NewPassword)
                .StrongPassword();

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.NewPassword).WithMessage(ValidationMessages.PasswordMismatch);
        }
    }
}