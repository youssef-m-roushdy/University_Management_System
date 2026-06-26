using FluentValidation;
using University_Management_System.Domain.Queries.SpecializationCourseQueries;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.SpecializationCourse
{
    public class SpecializationCourseFilterQueriesValidator : AbstractValidator<SpecializationCourseFilterQueries>
    {
        public SpecializationCourseFilterQueriesValidator()
        {
            // ─── Inherit Pagination Validation ──────────────────────────────
            Include(new SearchPaginationQueryValidator());

            RuleFor(x => x.SpecializationId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId)
                .When(x => x.SpecializationId.HasValue);

            RuleFor(x => x.CourseId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId)
                .When(x => x.CourseId.HasValue);

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

            RuleFor(x => x.SortBy)
                .Must(BeAllowedSortField)
                .WithMessage(ValidationMessages.InvalidSortField)
                .When(x => !string.IsNullOrEmpty(x.SortBy));
        }

        private static bool BeAllowedSortField(string? sortBy)
        {
            if (string.IsNullOrEmpty(sortBy)) return true;

            var allowedFields = new[]
            {
                "coursecode",
                "coursename",
                "role",
                "specialization"
            };

            return allowedFields.Contains(sortBy, StringComparer.OrdinalIgnoreCase);
        }
    }
}