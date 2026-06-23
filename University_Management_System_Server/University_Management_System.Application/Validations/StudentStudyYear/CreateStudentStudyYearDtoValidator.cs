using FluentValidation;
using University_Management_System.Application.Dtos.StudentStudyYearDtos;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.StudentStudyYear
{
    public class CreateStudentStudyYearDtoValidator : AbstractValidator<CreateStudentStudyYearDto>
    {
        public CreateStudentStudyYearDtoValidator()
        {
            // ─── Student ID ──────────────────────────────────────────────────
            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage(ValidationMessages.Required)
                .MaximumLength(450).WithMessage(ValidationMessages.MaxLength);

            // ─── Study Year ID ──────────────────────────────────────────────
            RuleFor(x => x.StudyYearId)
                .GreaterThan(0).WithMessage("Study year ID must be greater than 0");

            // ─── Level ──────────────────────────────────────────────────────
            RuleFor(x => x.Level)
                .IsInEnum()
                .WithMessage("Invalid level.");
        }
    }
}