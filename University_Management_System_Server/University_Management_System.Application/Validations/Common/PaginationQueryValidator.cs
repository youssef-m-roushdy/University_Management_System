using FluentValidation;
using University_Management_System.Domain.Queries;

namespace University_Management_System.Application.Validations.Common
{
    public class PaginationQueryValidator : AbstractValidator<PaginationQuery>
    {
        public PaginationQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1).WithMessage("Page size must be at least 1")
                .LessThanOrEqualTo(50).WithMessage("Page size must not exceed 50");

            RuleFor(x => x.SortBy)
                .MaximumLength(50).WithMessage("Sort by field must not exceed 50 characters")
                .When(x => !string.IsNullOrEmpty(x.SortBy));

            RuleFor(x => x.SearchTerm)
                .MaximumLength(100).WithMessage("Search term must not exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.SearchTerm));

            RuleFor(x => x.SortDirection)
                .IsInEnum().WithMessage("Invalid sort direction. Must be Ascending or Descending");
        }
    }
}