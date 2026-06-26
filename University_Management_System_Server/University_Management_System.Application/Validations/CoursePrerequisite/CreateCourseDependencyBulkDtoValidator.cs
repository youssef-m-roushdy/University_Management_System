using FluentValidation;
using University_Management_System.Application.Dtos.CoursePrerequisiteDtos;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.CoursePrerequisite
{
    public class CreateCourseDependencyBulkDtoValidator : AbstractValidator<CreateCourseDependencyBulkDto>
    {
        public CreateCourseDependencyBulkDtoValidator()
        {
            RuleFor(x => x.PrerequisiteCourseId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);

            RuleFor(x => x.DependentCourseIds)
                .NotNull().WithMessage(ValidationMessages.Required)
                .Must(list => list != null && list.Any()).WithMessage("At least one dependent course is required");

            RuleFor(x => x.DependentCourseIds)
                .Must(list => list == null || list.Distinct().Count() == list.Count)
                .WithMessage("Duplicate dependent course IDs are not allowed")
                .When(x => x.DependentCourseIds != null);

            RuleFor(x => x.DependentCourseIds)
                .Must(list => list == null || !list.Contains(0))
                .WithMessage("Invalid dependent course ID")
                .When(x => x.DependentCourseIds != null);

            RuleFor(x => x)
                .Must(x => x.DependentCourseIds == null || !x.DependentCourseIds.Contains(x.PrerequisiteCourseId))
                .WithMessage("A course cannot depend on itself.")
                .When(x => x.DependentCourseIds != null);
        }
    }
}