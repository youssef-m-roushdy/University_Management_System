using FluentValidation;
using University_Management_System.Application.Commands.Semesters;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.Semesters
{
    public class CreateSemesterCommandValidator : AbstractValidator<CreateStudyYearSemesterCommand>
    {
        public CreateSemesterCommandValidator()
        {
            RuleFor(x => x.StudyYearId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);

            RuleFor(x => x.SemesterDto)
                .NotNull().WithMessage(ValidationMessages.Required)
                .SetValidator(new CreateSemesterDtoValidator())
                .When(x => x.SemesterDto != null);
        }
    }
}