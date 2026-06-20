using FluentValidation;
using University_Management_System.Application.Dtos.AuthDtos;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.Auth
{
    public class ResendVerificationDtoValidator : AbstractValidator<ResendVerificationDto>
    {
        public ResendVerificationDtoValidator()
        {
            RuleFor(x => x.Email)
                .ValidEmail();
        }
    }
}