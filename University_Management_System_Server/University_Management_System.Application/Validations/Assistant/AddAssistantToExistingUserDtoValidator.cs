using FluentValidation;
using University_Management_System.Application.Dtos.AssistantDtos;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.Assistant
{
    public class AddAssistantToExistingUserDtoValidator : AbstractValidator<AddAssistantToExistingUserDto>
    {
        public AddAssistantToExistingUserDtoValidator()
        {
            // ─── User Email ────────────────────────────────────────────────────
            RuleFor(x => x.UserEmail)
                .ValidEmail();

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);         
        }
    }
}