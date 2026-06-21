using FluentValidation;
using University_Management_System.Domain.Queries;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.StudyYear
{
    public class StudyYearStudentQueriesValidator : AbstractValidator<StudyYearStudentQueries>
    {
        public StudyYearStudentQueriesValidator()
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

            RuleFor(x => x.AcademicCode)
                .MaximumLength(50).WithMessage(ValidationMessages.MaxLength)
                .When(x => !string.IsNullOrEmpty(x.AcademicCode));

            RuleFor(x => x.Level)
                .IsInEnum().WithMessage(ValidationMessages.InvalidEnumValue)
                .When(x => x.Level.HasValue);

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId)
                .When(x => x.DepartmentId.HasValue);

            RuleFor(x => x.MinGPA)
                .InclusiveBetween(0, 4).WithMessage(ValidationMessages.GpaRange)
                .When(x => x.MinGPA.HasValue);

            RuleFor(x => x.MaxGPA)
                .InclusiveBetween(0, 4).WithMessage(ValidationMessages.GpaRange)
                .When(x => x.MaxGPA.HasValue);

            RuleFor(x => x)
                .Must(x => !x.MinGPA.HasValue || !x.MaxGPA.HasValue || x.MinGPA <= x.MaxGPA)
                .WithMessage(ValidationMessages.GpaMinMax)
                .OverridePropertyName(nameof(StudyYearStudentQueries.MaxGPA));

            RuleFor(x => x)
                .Must(x => !x.EnrolledFrom.HasValue || !x.EnrolledTo.HasValue || x.EnrolledFrom <= x.EnrolledTo)
                .WithMessage(ValidationMessages.DateRangeMinMax)
                .OverridePropertyName(nameof(StudyYearStudentQueries.EnrolledTo));
        }
    }
}