using FluentValidation;
using University_Management_System.Domain.Queries.StudentQueries;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.Student
{
    public class StudentDepartmentQueriesValidator : AbstractValidator<StudentDepartmentQueries>
    {
        public StudentDepartmentQueriesValidator()
        {
            // ─── Inherit Pagination Validation ──────────────────────────────
            Include(new SearchPaginationQueryValidator());

            // ─── Level ─────────────────────────────────────────────────────────
            RuleFor(x => x.Level)
                .IsInEnum().WithMessage("Invalid level value")
                .When(x => x.Level.HasValue);

            // ─── Gender ──────────────────────────────────────────────────────
            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage("Invalid gender value")
                .When(x => x.Gender.HasValue);

            // ─── Specialization Search ────────────────────────────────────────
            RuleFor(x => x.SpecializationSearch)
                .MaximumLength(100).WithMessage("Specialization search term must not exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.SpecializationSearch));

            // ─── GPA ──────────────────────────────────────────────────────────
            RuleFor(x => x.MinGPA)
                .InclusiveBetween(0, 4).WithMessage("Minimum GPA must be between 0 and 4")
                .When(x => x.MinGPA.HasValue);

            RuleFor(x => x.MaxGPA)
                .InclusiveBetween(0, 4).WithMessage("Maximum GPA must be between 0 and 4")
                .When(x => x.MaxGPA.HasValue);

            RuleFor(x => x)
                .Must(x => !x.MinGPA.HasValue || !x.MaxGPA.HasValue || x.MinGPA <= x.MaxGPA)
                .WithMessage(ValidationMessages.GpaMinMax);

            // ─── Credits ──────────────────────────────────────────────────────
            RuleFor(x => x.MinTotalCredits)
                .GreaterThanOrEqualTo(0).WithMessage("Minimum total credits must be at least 0")
                .When(x => x.MinTotalCredits.HasValue);

            RuleFor(x => x.MaxTotalCredits)
                .GreaterThanOrEqualTo(0).WithMessage("Maximum total credits must be at least 0")
                .When(x => x.MaxTotalCredits.HasValue);

            RuleFor(x => x)
                .Must(x => !x.MinTotalCredits.HasValue || !x.MaxTotalCredits.HasValue || x.MinTotalCredits <= x.MaxTotalCredits)
                .WithMessage("Minimum total credits must be less than or equal to maximum total credits");

            RuleFor(x => x.MinAllowedCredits)
                .GreaterThanOrEqualTo(0).WithMessage("Minimum allowed credits must be at least 0")
                .When(x => x.MinAllowedCredits.HasValue);

            RuleFor(x => x.MaxAllowedCredits)
                .GreaterThanOrEqualTo(0).WithMessage("Maximum allowed credits must be at least 0")
                .When(x => x.MaxAllowedCredits.HasValue);

            RuleFor(x => x)
                .Must(x => !x.MinAllowedCredits.HasValue || !x.MaxAllowedCredits.HasValue || x.MinAllowedCredits <= x.MaxAllowedCredits)
                .WithMessage("Minimum allowed credits must be less than or equal to maximum allowed credits");

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
                "academiccode",
                "level",
                "gpa",
                "totalcredits",
                "specialization"
            };

            return allowedFields.Contains(sortBy, StringComparer.OrdinalIgnoreCase);
        }
    }
}