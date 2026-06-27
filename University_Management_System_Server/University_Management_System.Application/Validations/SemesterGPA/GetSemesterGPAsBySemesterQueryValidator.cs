using FluentValidation;
using University_Management_System.Application.Queries.SemesterGPAs;

namespace University_Management_System.Application.Validations.SemesterGPA
{
    public class GetSemesterGPAsBySemesterQueryValidator : AbstractValidator<GetSemesterGPAsBySemesterQuery>
    {
        public GetSemesterGPAsBySemesterQueryValidator()
        {
            RuleFor(x => x.SemesterId)
                .GreaterThan(0).WithMessage("Semester ID must be greater than 0");

            RuleFor(x => x.Filter)
                .NotNull().WithMessage("Filter is required")
                .SetValidator(new SemesterGPAFilterInSemesterQueriesValidator());
        }
    }
}