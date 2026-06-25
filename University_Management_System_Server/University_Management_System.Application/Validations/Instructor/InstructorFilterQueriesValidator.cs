using FluentValidation;
using University_Management_System.Domain.Queries.InstructorQueries;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.Instructor
{
    public class InstructorFilterQueriesValidator : AbstractValidator<InstructorFilterQueries>
    {
        public InstructorFilterQueriesValidator()
        {
            // ─── Inherit Pagination Validation ──────────────────────────────
            Include(new SearchPaginationQueryValidator());

            // ─── Name ──────────────────────────────────────────────────────────
            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage(ValidationMessages.MaxLength)
                .When(x => !string.IsNullOrEmpty(x.Name));

            // ─── Gender ──────────────────────────────────────────────────────
            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage(ValidationMessages.InvalidEnumValue)
                .When(x => x.Gender.HasValue);

            // ─── Department Search ────────────────────────────────────────────
            RuleFor(x => x.DepartmentSearch)
                .MaximumLength(100).WithMessage(ValidationMessages.MaxLength)
                .When(x => !string.IsNullOrEmpty(x.DepartmentSearch));

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
                "email",
                "department",
                "createdat"
            };

            return allowedFields.Contains(sortBy, StringComparer.OrdinalIgnoreCase);
        }
    }
}