using FluentValidation;
using University_Management_System.Application.Commands.Courses;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.Course
{
    public class DeleteCourseCommandValidator : AbstractValidator<DeleteCourseCommand>
    {
        public DeleteCourseCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);
        }
    }
}