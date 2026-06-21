using FluentValidation;
using University_Management_System.Domain.Queries;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.StudyYear
{
    public class StudyYearQueriesValidator : AbstractValidator<StudyYearQueries>
    {
        public StudyYearQueriesValidator()
        {
            Include(new PaginationQueryValidator());

            When(x => x.StartYear.HasValue, () =>
            {
                RuleFor(x => x.StartYear)
                    .GreaterThan(2000).WithMessage("Start year must be greater than 2000")
                    .LessThan(2100).WithMessage("Start year must be less than 2100");
            });

            When(x => x.EndYear.HasValue, () =>
            {
                RuleFor(x => x.EndYear)
                    .GreaterThan(2000).WithMessage("End year must be greater than 2000")
                    .LessThan(2100).WithMessage("End year must be less than 2100");
            });

            When(x => x.MinYear.HasValue, () =>
            {
                RuleFor(x => x.MinYear)
                    .GreaterThan(2000).WithMessage("Min year must be greater than 2000")
                    .LessThan(2100).WithMessage("Min year must be less than 2100");
            });

            When(x => x.MaxYear.HasValue, () =>
            {
                RuleFor(x => x.MaxYear)
                    .GreaterThan(2000).WithMessage("Max year must be greater than 2000")
                    .LessThan(2100).WithMessage("Max year must be less than 2100");
            });

            RuleFor(x => x)
                .Must(x => !x.MinYear.HasValue || !x.MaxYear.HasValue || x.MinYear <= x.MaxYear)
                .WithMessage("Min year must be less than or equal to Max year");

            RuleFor(x => x.SortBy)
                .Must(BeAllowedSortField)
                .WithMessage(ValidationMessages.InvalidSortField)
                .When(x => !string.IsNullOrEmpty(x.SortBy));
        }

        private static bool BeAllowedSortField(string? sortBy)
        {
            if (string.IsNullOrEmpty(sortBy)) return true;

            var allowedFields = new[] { "StartYear", "EndYear", "IsCurrent", "CreatedAt" };
            return allowedFields.Contains(sortBy, StringComparer.OrdinalIgnoreCase);
        }
    }
}