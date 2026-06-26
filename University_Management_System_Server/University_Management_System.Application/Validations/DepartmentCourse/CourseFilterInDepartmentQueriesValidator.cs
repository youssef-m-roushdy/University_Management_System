using FluentValidation;
using University_Management_System.Domain.Queries.DepartmentCourseQueries;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.DepartmentCourse
{
    public class CourseFilterInDepartmentQueriesValidator : AbstractValidator<CourseFilterInDepartmentQueries>
    {
        public CourseFilterInDepartmentQueriesValidator()
        {
            RuleFor(x => x.Role)
                .IsInEnum().WithMessage(ValidationMessages.InvalidEnumValue)
                .When(x => x.Role.HasValue);

            RuleFor(x => x.CourseName)
                .MaximumLength(200).WithMessage(ValidationMessages.MaxLength)
                .When(x => !string.IsNullOrEmpty(x.CourseName));

            RuleFor(x => x.CourseCode)
                .MaximumLength(20).WithMessage(ValidationMessages.MaxLength)
                .When(x => !string.IsNullOrEmpty(x.CourseCode));

            RuleFor(x => x.MinCredits)
                .GreaterThanOrEqualTo(0).WithMessage("Minimum credits must be at least 0")
                .When(x => x.MinCredits.HasValue);

            RuleFor(x => x.MaxCredits)
                .GreaterThanOrEqualTo(0).WithMessage("Maximum credits must be at least 0")
                .When(x => x.MaxCredits.HasValue);

            RuleFor(x => x)
                .Must(x => !x.MinCredits.HasValue || !x.MaxCredits.HasValue || x.MinCredits <= x.MaxCredits)
                .WithMessage("Minimum credits must be less than or equal to maximum credits");

            // ─── SearchTerm is not inherited here, but we can validate if added ──
        }
    }
}