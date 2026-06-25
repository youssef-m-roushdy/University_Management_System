using FluentValidation;
using University_Management_System.Application.Commands.Instructors;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.Instructor
{
    public class DeleteInstructorCommandValidator : AbstractValidator<DeleteInstructorCommand>
    {
        public DeleteInstructorCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(ValidationMessages.Required);
        }
    }
}