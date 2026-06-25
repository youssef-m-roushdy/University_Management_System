using FluentValidation;
using University_Management_System.Application.Dtos.AssistantDtos;
using University_Management_System.Application.Validations.Common;
using University_Management_System.Application.Validations.User;

namespace University_Management_System.Application.Validations.Assistant
{
    public class CreateAssistantDtoValidator : AbstractValidator<CreateAssistantDto>
    {
        public CreateAssistantDtoValidator()
        {
            Include(new CreateUserDtoValidator());

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);
        }
    }
}