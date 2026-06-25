using FluentValidation;
using University_Management_System.Application.Commands.Assistants;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.Assistant
{
    public class DeleteAssistantCommandValidator : AbstractValidator<DeleteAssistantCommand>
    {
        public DeleteAssistantCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(ValidationMessages.Required);
        }
    }
}