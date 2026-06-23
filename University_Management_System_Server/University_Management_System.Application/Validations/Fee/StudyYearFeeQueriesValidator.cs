using FluentValidation;
using University_Management_System.Domain.Queries;
using University_Management_System.Application.Validations.Common;
using University_Management_System.Domain.Queries.FeeQueries;

namespace University_Management_System.Application.Validations.StudyYear
{
    public class StudyYearFeeQueriesValidator : AbstractValidator<FeeStudyYearQueries>
    {
        public StudyYearFeeQueriesValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage(ValidationMessages.PageNumberMin);

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1).WithMessage(ValidationMessages.PageSizeMin)
                .LessThanOrEqualTo(50).WithMessage(ValidationMessages.PageSizeMax);

            RuleFor(x => x.SortBy)
                .MaximumLength(50).WithMessage(ValidationMessages.MaxLength)
                .When(x => !string.IsNullOrEmpty(x.SortBy));

            RuleFor(x => x.SortDirection)
                .IsInEnum().WithMessage(ValidationMessages.InvalidSortDirection);

            RuleFor(x => x.Level)
                .IsInEnum().WithMessage(ValidationMessages.InvalidEnumValue)
                .When(x => x.Level.HasValue);

            RuleFor(x => x.FeeType)
                .IsInEnum().WithMessage(ValidationMessages.InvalidEnumValue)
                .When(x => x.FeeType.HasValue);

            RuleFor(x => x.MinAmount)
                .GreaterThanOrEqualTo(0).WithMessage(ValidationMessages.AmountPositive)
                .When(x => x.MinAmount.HasValue);

            RuleFor(x => x.MaxAmount)
                .GreaterThanOrEqualTo(0).WithMessage(ValidationMessages.AmountPositive)
                .When(x => x.MaxAmount.HasValue);

            RuleFor(x => x)
                .Must(x => !x.MinAmount.HasValue || !x.MaxAmount.HasValue || x.MinAmount <= x.MaxAmount)
                .WithMessage(ValidationMessages.AmountMinMax)
                .OverridePropertyName(nameof(FeeStudyYearQueries.MaxAmount));
        }
    }
}