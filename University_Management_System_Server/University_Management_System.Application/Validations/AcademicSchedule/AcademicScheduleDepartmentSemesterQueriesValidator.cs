using FluentValidation;
using University_Management_System.Domain.Queries.AcademicScheduleQueries;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.AcademicSchedule
{
    public class AcademicScheduleDepartmentSemesterQueriesValidator : AbstractValidator<AcademicScheduleDepartmentSemesterQueries>
    {
        public AcademicScheduleDepartmentSemesterQueriesValidator()
        {
            Include(new SearchPaginationQueryValidator());

            RuleFor(x => x.ScheduleDateFrom)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Schedule date from cannot be in the future")
                .When(x => x.ScheduleDateFrom.HasValue);

            RuleFor(x => x.ScheduleDateTo)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Schedule date to cannot be in the future")
                .When(x => x.ScheduleDateTo.HasValue);

            RuleFor(x => x)
                .Must(x => !x.ScheduleDateFrom.HasValue || !x.ScheduleDateTo.HasValue || x.ScheduleDateFrom <= x.ScheduleDateTo)
                .WithMessage("Schedule date from must be less than or equal to schedule date to");

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
                "title",
                "scheduledate",
                "department"
            };

            return allowedFields.Contains(sortBy, StringComparer.OrdinalIgnoreCase);
        }
    }
}