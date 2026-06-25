using FluentValidation;
using University_Management_System.Application.Dtos.AssistantDtos;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.Assistant
{
    public class UpdateAssistantDtoValidator : AbstractValidator<UpdateAssistantDto>
    {
        public UpdateAssistantDtoValidator()
        {
            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);
        }
    }
}