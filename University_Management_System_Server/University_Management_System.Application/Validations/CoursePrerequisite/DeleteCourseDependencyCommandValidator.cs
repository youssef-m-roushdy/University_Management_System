using FluentValidation;
using University_Management_System.Application.Commands.CoursePrerequisites;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.CoursePrerequisite
{
    public class DeleteCourseDependencyCommandValidator : AbstractValidator<DeleteCourseDependencyCommand>
    {
        public DeleteCourseDependencyCommandValidator()
        {
            RuleFor(x => x.CourseId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);

            RuleFor(x => x.DependencyCourseId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);

            RuleFor(x => x)
                .Must(x => x.CourseId != x.DependencyCourseId)
                .WithMessage("Course ID and Dependency Course ID cannot be the same.");
        }
    }
}