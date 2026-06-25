using FluentValidation;
using University_Management_System.Domain.Queries.CourseQueries;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.Course
{
    public class CourseFilterQueriesValidator : AbstractValidator<CourseFilterQueries>
    {
        public CourseFilterQueriesValidator()
        {
            // ─── Inherit Pagination Validation ──────────────────────────────
            Include(new SearchPaginationQueryValidator());

            // ─── Status ──────────────────────────────────────────────────────
            RuleFor(x => x.Status)
                .IsInEnum().WithMessage(ValidationMessages.InvalidEnumValue)
                .When(x => x.Status.HasValue);

            // ─── Code ──────────────────────────────────────────────────────────
            RuleFor(x => x.Code)
                .MaximumLength(20).WithMessage(ValidationMessages.MaxLength)
                .When(x => !string.IsNullOrEmpty(x.Code));

            // ─── Name ──────────────────────────────────────────────────────────
            RuleFor(x => x.Name)
                .MaximumLength(200).WithMessage(ValidationMessages.MaxLength)
                .When(x => !string.IsNullOrEmpty(x.Name));

            // ─── Department ──────────────────────────────────────────────────
            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId)
                .When(x => x.DepartmentId.HasValue);

            // ─── Credits ──────────────────────────────────────────────────────
            RuleFor(x => x.MinCredits)
                .GreaterThanOrEqualTo(0).WithMessage("Minimum credits must be at least 0")
                .When(x => x.MinCredits.HasValue);

            RuleFor(x => x.MaxCredits)
                .GreaterThanOrEqualTo(0).WithMessage("Maximum credits must be at least 0")
                .When(x => x.MaxCredits.HasValue);

            RuleFor(x => x)
                .Must(x => !x.MinCredits.HasValue || !x.MaxCredits.HasValue || x.MinCredits <= x.MaxCredits)
                .WithMessage("Minimum credits must be less than or equal to maximum credits");

            // ─── Sort By Allowed Values ──────────────────────────────────────
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
                "name",
                "code",
                "credits",
                "status"
            };

            return allowedFields.Contains(sortBy, StringComparer.OrdinalIgnoreCase);
        }
    }
}