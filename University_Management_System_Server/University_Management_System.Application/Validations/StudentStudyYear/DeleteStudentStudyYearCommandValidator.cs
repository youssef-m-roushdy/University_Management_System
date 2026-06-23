using FluentValidation;
using University_Management_System.Application.Commands.StudentStudyYears;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.StudentStudyYear
{
    public class DeleteStudentStudyYearCommandValidator : AbstractValidator<DeleteStudentStudyYearCommand>
    {
        public DeleteStudentStudyYearCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Enrollment ID must be greater than 0");
        }
    }
}