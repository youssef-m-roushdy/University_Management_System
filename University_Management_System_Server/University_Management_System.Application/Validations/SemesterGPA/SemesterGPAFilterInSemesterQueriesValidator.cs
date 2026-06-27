using FluentValidation;
using University_Management_System.Domain.Queries.SemesterGPAQueries;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.SemesterGPA
{
    public class SemesterGPAFilterInSemesterQueriesValidator : AbstractValidator<SemesterGPAFilterInSemesterQueries>
    {
        public SemesterGPAFilterInSemesterQueriesValidator()
        {
            // Inherit pagination validation
            Include(new SearchPaginationQueryValidator());

            // Department ID
            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("Department ID must be greater than 0")
                .When(x => x.DepartmentId.HasValue);

            // GPA Range
            RuleFor(x => x.MinGPA)
                .InclusiveBetween(0, 4).WithMessage("Minimum GPA must be between 0 and 4")
                .When(x => x.MinGPA.HasValue);

            RuleFor(x => x.MaxGPA)
                .InclusiveBetween(0, 4).WithMessage("Maximum GPA must be between 0 and 4")
                .When(x => x.MaxGPA.HasValue);

            RuleFor(x => x)
                .Must(x => !x.MinGPA.HasValue || !x.MaxGPA.HasValue || x.MinGPA <= x.MaxGPA)
                .WithMessage("Minimum GPA must be less than or equal to maximum GPA");

            // Credit Hours Range
            RuleFor(x => x.MinCreditHours)
                .GreaterThanOrEqualTo(0).WithMessage("Minimum credit hours must be at least 0")
                .When(x => x.MinCreditHours.HasValue);

            RuleFor(x => x.MaxCreditHours)
                .GreaterThanOrEqualTo(0).WithMessage("Maximum credit hours must be at least 0")
                .When(x => x.MaxCreditHours.HasValue);

            RuleFor(x => x)
                .Must(x => !x.MinCreditHours.HasValue || !x.MaxCreditHours.HasValue || x.MinCreditHours <= x.MaxCreditHours)
                .WithMessage("Minimum credit hours must be less than or equal to maximum credit hours");

            // Sort By Allowed Values
            RuleFor(x => x.SortBy)
                .Must(BeAllowedSortField)
                .WithMessage("Invalid sort field. Allowed fields: student, academiccode, gpa, credithours, studyyear, calculatedat")
                .When(x => !string.IsNullOrEmpty(x.SortBy));
        }

        private static bool BeAllowedSortField(string? sortBy)
        {
            if (string.IsNullOrEmpty(sortBy)) return true;

            var allowedFields = new[]
            {
                "student",
                "academiccode",
                "gpa",
                "credithours",
                "studyyear",
                "calculatedat"
            };

            return allowedFields.Contains(sortBy, StringComparer.OrdinalIgnoreCase);
        }
    }
}