using FluentValidation;
using University_Management_System.Domain.Queries.AssistantQueries;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.Assistant
{
    public class AssistantFilterQueriesValidator : AbstractValidator<AssistantFilterQueries>
    {
        public AssistantFilterQueriesValidator()
        {
            Include(new SearchPaginationQueryValidator());

            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage(ValidationMessages.MaxLength)
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage(ValidationMessages.InvalidEnumValue)
                .When(x => x.Gender.HasValue);

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId)
                .When(x => x.DepartmentId.HasValue);

            RuleFor(x => x.DepartmentSearch)
                .MaximumLength(100).WithMessage(ValidationMessages.MaxLength)
                .When(x => !string.IsNullOrEmpty(x.DepartmentSearch));

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
                "email",
                "department",
                "createdat"
            };

            return allowedFields.Contains(sortBy, StringComparer.OrdinalIgnoreCase);
        }
    }
}