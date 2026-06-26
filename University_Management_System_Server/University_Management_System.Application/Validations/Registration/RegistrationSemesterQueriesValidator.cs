using FluentValidation;
using University_Management_System.Domain.Queries.RegistrationQueries;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.Registration
{
    public class RegistrationSemesterQueriesValidator : AbstractValidator<RegistrationSemesterQueries>
    {
        public RegistrationSemesterQueriesValidator()
        {
            Include(new SearchPaginationQueryValidator());

            RuleFor(x => x.StudentName)
                .MaximumLength(100).WithMessage(ValidationMessages.MaxLength)
                .When(x => !string.IsNullOrEmpty(x.StudentName));

            RuleFor(x => x.AcademicCode)
                .MaximumLength(50).WithMessage(ValidationMessages.MaxLength)
                .When(x => !string.IsNullOrEmpty(x.AcademicCode));

            RuleFor(x => x.CourseName)
                .MaximumLength(200).WithMessage(ValidationMessages.MaxLength)
                .When(x => !string.IsNullOrEmpty(x.CourseName));

            RuleFor(x => x.CourseCode)
                .MaximumLength(20).WithMessage(ValidationMessages.MaxLength)
                .When(x => !string.IsNullOrEmpty(x.CourseCode));

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage(ValidationMessages.InvalidEnumValue)
                .When(x => x.Status.HasValue);

            RuleFor(x => x.IsPassed)
                .NotNull().When(x => x.IsPassed.HasValue);

            RuleFor(x => x.Progress)
                .IsInEnum().WithMessage(ValidationMessages.InvalidEnumValue)
                .When(x => x.Progress.HasValue);

            RuleFor(x => x.Grade)
                .IsInEnum().WithMessage(ValidationMessages.InvalidEnumValue)
                .When(x => x.Grade.HasValue);

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
                "student",
                "course",
                "status",
                "progress",
                "grade",
                "registeredat"
            };

            return allowedFields.Contains(sortBy, StringComparer.OrdinalIgnoreCase);
        }
    }
}