using FluentValidation;
using University_Management_System.Application.Dtos.CoursePrerequisiteDtos;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.CoursePrerequisite
{
    public class CreateCoursePrerequisiteBulkDtoValidator : AbstractValidator<CreateCoursePrerequisiteBulkDto>
    {
        public CreateCoursePrerequisiteBulkDtoValidator()
        {
            RuleFor(x => x.CourseId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);

            RuleFor(x => x.PrerequisiteCourseIds)
                .NotNull().WithMessage(ValidationMessages.Required)
                .Must(list => list != null && list.Any()).WithMessage("At least one prerequisite course is required");

            RuleFor(x => x.PrerequisiteCourseIds)
                .Must(list => list == null || list.Distinct().Count() == list.Count)
                .WithMessage("Duplicate prerequisite course IDs are not allowed")
                .When(x => x.PrerequisiteCourseIds != null);

            RuleFor(x => x.PrerequisiteCourseIds)
                .Must(list => list == null || !list.Contains(0))
                .WithMessage("Invalid prerequisite course ID")
                .When(x => x.PrerequisiteCourseIds != null);

            RuleFor(x => x)
                .Must(x => x.PrerequisiteCourseIds == null || !x.PrerequisiteCourseIds.Contains(x.CourseId))
                .WithMessage("A course cannot be a prerequisite of itself.")
                .When(x => x.PrerequisiteCourseIds != null);
        }
    }
}