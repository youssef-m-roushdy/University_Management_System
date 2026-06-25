using FluentValidation;
using University_Management_System.Domain.Queries.AssistantQueries;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.Assistant
{
    public class AssistantDepartmentQueriesValidator : AbstractValidator<AssistantDepartmentQueries>
    {
        public AssistantDepartmentQueriesValidator()
        {
            Include(new SearchPaginationQueryValidator());

            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage(ValidationMessages.MaxLength)
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage(ValidationMessages.InvalidEnumValue)
                .When(x => x.Gender.HasValue);

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
                "createdat"
            };

            return allowedFields.Contains(sortBy, StringComparer.OrdinalIgnoreCase);
        }
    }
}