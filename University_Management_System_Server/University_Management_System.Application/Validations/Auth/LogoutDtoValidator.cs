using FluentValidation;
using University_Management_System.Application.Dtos.AuthDtos;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.Auth
{
    public class LogoutDtoValidator : AbstractValidator<LogoutDto>
    {
        public LogoutDtoValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage(ValidationMessages.Required);
        }
    }
}