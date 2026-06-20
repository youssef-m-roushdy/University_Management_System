using FluentValidation;
using University_Management_System.Application.Dtos.AuthDtos;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.Auth
{
    public class RefreshTokenDtoValidator : AbstractValidator<RefreshTokenDto>
    {
        public RefreshTokenDtoValidator()
        {
            RuleFor(x => x.AccessToken)
                .NotEmpty().WithMessage("Access token is required");

            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage(ValidationMessages.Required);
        }
    }
}