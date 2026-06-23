using FluentValidation;
using University_Management_System.Domain.Queries;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.StudentStudyYear
{
    public class StudyYearStudentQueriesValidator : AbstractValidator<StudyYearStudentQueries>
    {
        public StudyYearStudentQueriesValidator()
        {
            // ✅ Inherit all pagination rules (PageNumber, PageSize, SortBy, SortDirection, SearchTerm)
            Include(new SearchPaginationQueryValidator());

            // ─── Department Code ──────────────────────────────────────────────
            RuleFor(x => x.DepartmentCode)
                .MaximumLength(50).WithMessage("Department code must not exceed 50 characters")
                .When(x => !string.IsNullOrEmpty(x.DepartmentCode));
            
            // ─── Department Name ──────────────────────────────────────────────
            RuleFor(x => x.DepartmentName)
                .MaximumLength(100).WithMessage("Department name must not exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.DepartmentName));

            // ─── GPA ────────────────────────────────────────────────────────
            RuleFor(x => x.MinGPA)
                .InclusiveBetween(0, 4).WithMessage("Minimum GPA must be between 0 and 4")
                .When(x => x.MinGPA.HasValue);

            RuleFor(x => x.MaxGPA)
                .InclusiveBetween(0, 4).WithMessage("Maximum GPA must be between 0 and 4")
                .When(x => x.MaxGPA.HasValue);

            RuleFor(x => x)
                .Must(x => !x.MinGPA.HasValue || !x.MaxGPA.HasValue || x.MinGPA <= x.MaxGPA)
                .WithMessage(ValidationMessages.GpaMinMax);

            // ─── Dates ──────────────────────────────────────────────────────
            RuleFor(x => x.EnrolledFrom)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Enrolled from date cannot be in the future")
                .When(x => x.EnrolledFrom.HasValue);

            RuleFor(x => x.EnrolledTo)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Enrolled to date cannot be in the future")
                .When(x => x.EnrolledTo.HasValue);

            RuleFor(x => x)
                .Must(x => !x.EnrolledFrom.HasValue || !x.EnrolledTo.HasValue || x.EnrolledFrom <= x.EnrolledTo)
                .WithMessage("Enrolled from date must be before enrolled to date");

            // ─── Sort By Allowed Values ────────────────────────────────────
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
                "enrolledat",
                "department"
            };

            return allowedFields.Contains(sortBy, StringComparer.OrdinalIgnoreCase);
        }
    }
}