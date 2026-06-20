using FluentValidation;
using University_Management_System.Application.Dtos.AuthDtos;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.Auth
{
    public class ForgotPasswordDtoValidator : AbstractValidator<ForgotPasswordDto>
    {
        public ForgotPasswordDtoValidator()
        {
            RuleFor(x => x.Email)
                .ValidEmail();
        }
    }
}