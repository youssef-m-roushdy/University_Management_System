using FluentValidation;
using University_Management_System.Domain.Queries;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.Student
{
    public class StudentQueriesValidator : AbstractValidator<StudentQueries>
    {
        public StudentQueriesValidator()
        {
            // ─── Include Pagination Validation ───────────────────────────────
            Include(new PaginationQueryValidator());

            // ─── Academic Code Validation ─────────────────────────────────────
            RuleFor(x => x.AcademicCode)
                .MaximumLength(20).WithMessage(ValidationMessages.MaxLength)
                .Matches(@"^[A-Za-z0-9\-]+$")
                .WithMessage("Academic code can only contain letters, numbers, and hyphens")
                .When(x => !string.IsNullOrEmpty(x.AcademicCode));

            // ─── Name Validation ──────────────────────────────────────────────
            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage(ValidationMessages.MaxLength)
                .When(x => !string.IsNullOrEmpty(x.Name));

            // ─── Gender Validation ────────────────────────────────────────────
            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage(ValidationMessages.InvalidEnumValue)
                .When(x => x.Gender.HasValue);

            // ─── Level Validation ─────────────────────────────────────────────
            RuleFor(x => x.Level)
                .IsInEnum().WithMessage("Invalid level value")
                .When(x => x.Level.HasValue);

            // ─── Department ID Validation ────────────────────────────────────
            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("Department ID must be greater than 0")
                .When(x => x.DepartmentId.HasValue);

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

            // ─── Credits Validation ──────────────────────────────────────────
            RuleFor(x => x.MinCredits)
                .GreaterThanOrEqualTo(0).WithMessage("Minimum credits must be at least 0")
                .When(x => x.MinCredits.HasValue);

            RuleFor(x => x.MaxCredits)
                .GreaterThanOrEqualTo(0).WithMessage("Maximum credits must be at least 0")
                .When(x => x.MaxCredits.HasValue);

            RuleFor(x => x)
                .Must(x => !x.MinCredits.HasValue || !x.MaxCredits.HasValue || x.MinCredits <= x.MaxCredits)
                .WithMessage("Minimum credits must be less than or equal to Maximum credits");

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