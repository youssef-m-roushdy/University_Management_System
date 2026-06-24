using FluentValidation;
using University_Management_System.Domain.Queries;
using University_Management_System.Application.Validations.Common;
using University_Management_System.Domain.Queries.StudentQueries;

namespace University_Management_System.Application.Validations.Student
{
    public class StudentQueriesValidator : AbstractValidator<StudentFilterQueries>
    {
        public StudentQueriesValidator()
        {
            // ─── Include Pagination Validation ───────────────────────────────
            Include(new SearchPaginationQueryValidator());

            // ─── Gender Validation ────────────────────────────────────────────
            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage(ValidationMessages.InvalidEnumValue)
                .When(x => x.Gender.HasValue);

            // ─── Level Validation ─────────────────────────────────────────────
            RuleFor(x => x.Level)
                .IsInEnum().WithMessage("Invalid level value")
                .When(x => x.Level.HasValue);

            // ─── GPA Validation ──────────────────────────────────────────────
            RuleFor(x => x.MinGPA)
                .GreaterThanOrEqualTo(0).WithMessage("Minimum GPA must be at least 0")
                .LessThanOrEqualTo(4).WithMessage("Minimum GPA must not exceed 4")
                .When(x => x.MinGPA.HasValue);

            RuleFor(x => x.MaxGPA)
                .GreaterThanOrEqualTo(0).WithMessage("Maximum GPA must be at least 0")
                .LessThanOrEqualTo(4).WithMessage("Maximum GPA must not exceed 4")
                .When(x => x.MaxGPA.HasValue);

            RuleFor(x => x)
                .Must(x => !x.MinGPA.HasValue || !x.MaxGPA.HasValue || x.MinGPA <= x.MaxGPA)
                .WithMessage("Minimum GPA must be less than or equal to Maximum GPA");

            // ─── SortBy Allowed Values ───────────────────────────────────────
            RuleFor(x => x.SortBy)
                .Must(BeAllowedSortField)
                .WithMessage("Sort by field must be one of: Name, Email, Level, GPA, AcademicCode, CreatedAt")
                .When(x => !string.IsNullOrEmpty(x.SortBy));
        }

        private static bool BeAllowedSortField(string? sortBy)
        {
            if (string.IsNullOrEmpty(sortBy)) return true;

            var allowedFields = new[] { "Name", "Email", "Level", "GPA", "AcademicCode", "CreatedAt" };
            return allowedFields.Contains(sortBy, StringComparer.OrdinalIgnoreCase);
        }
    }
}