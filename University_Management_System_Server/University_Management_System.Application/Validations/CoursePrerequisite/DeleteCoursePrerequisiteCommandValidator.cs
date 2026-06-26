using FluentValidation;
using University_Management_System.Application.Commands.CoursePrerequisites;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.CoursePrerequisite
{
    public class DeleteCoursePrerequisiteCommandValidator : AbstractValidator<DeleteCoursePrerequisiteCommand>
    {
        public DeleteCoursePrerequisiteCommandValidator()
        {
            RuleFor(x => x.CourseId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);

            RuleFor(x => x.PrerequisiteCourseId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);

            RuleFor(x => x)
                .Must(x => x.CourseId != x.PrerequisiteCourseId)
                .WithMessage("Course ID and Prerequisite Course ID cannot be the same.");
        }
    }
}