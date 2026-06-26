using FluentValidation;
using University_Management_System.Application.Commands.Specializations;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.Specialization
{
    public class DeleteSpecializationCommandValidator : AbstractValidator<DeleteSpecializationCommand>
    {
        public DeleteSpecializationCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);
        }
    }
}