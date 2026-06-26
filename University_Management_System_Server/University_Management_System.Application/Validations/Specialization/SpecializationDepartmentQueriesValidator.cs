using FluentValidation;
using University_Management_System.Domain.Queries.SpecializationQueries;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.Specialization
{
    public class SpecializationDepartmentQueriesValidator : AbstractValidator<SpecializationDepartmentQueries>
    {
        public SpecializationDepartmentQueriesValidator()
        {
            // ─── Inherit Pagination Validation ──────────────────────────────
            Include(new SearchPaginationQueryValidator());

            // ─── Name ──────────────────────────────────────────────────────────
            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage(ValidationMessages.MaxLength)
                .When(x => !string.IsNullOrEmpty(x.Name));

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
                "studentcount",
                "coursecount"
            };

            return allowedFields.Contains(sortBy, StringComparer.OrdinalIgnoreCase);
        }
    }
}