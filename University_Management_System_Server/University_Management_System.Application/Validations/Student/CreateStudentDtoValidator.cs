using FluentValidation;
using University_Management_System.Application.Dtos.StudentDtos;
using University_Management_System.Application.Validations.Common;
using University_Management_System.Application.Validations.User;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Validations.Student
{
    public class CreateStudentDtoValidator : AbstractValidator<CreateStudentDto>
    {
        public CreateStudentDtoValidator()
        {
            // ─── Inherit User Validation ──────────────────────────────────────
            Include(new CreateUserDtoValidator());

            // ─── Academic Code ─────────────────────────────────────────────────
            RuleFor(x => x.AcademicCode)
                .NotEmpty().WithMessage(ValidationMessages.Required)
                .MaximumLength(50).WithMessage(ValidationMessages.MaxLength)
                .Matches(@"^[A-Za-z0-9]+$").WithMessage("Academic code must contain only letters and numbers");

            // ─── Level ─────────────────────────────────────────────────────────
            RuleFor(x => x.Level)
                .IsInEnum().WithMessage("Invalid level value")
                .Must(level => level != Levels.Graduate).WithMessage("Cannot create a student with Graduate level");

            // ─── Credits ──────────────────────────────────────────────────────
            RuleFor(x => x.TotalCredits)
                .GreaterThanOrEqualTo(0).WithMessage("Total credits must be at least 0");

            RuleFor(x => x.AllowedCredits)
                .GreaterThanOrEqualTo(0).WithMessage("Allowed credits must be at least 0");

            RuleFor(x => x)
                .Must(x => x.TotalCredits <= x.AllowedCredits)
                .WithMessage("Total credits cannot exceed allowed credits");

            // ─── GPA ──────────────────────────────────────────────────────────
            RuleFor(x => x.TotalGPA)
                .InclusiveBetween(0, 4).WithMessage("GPA must be between 0 and 4");

            // ─── Department ──────────────────────────────────────────────────
            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("Department ID must be greater than 0");

            // ─── Specialization ──────────────────────────────────────────────
            RuleFor(x => x.SpecializationId)
                .GreaterThan(0).WithMessage("Specialization ID must be greater than 0")
                .When(x => x.SpecializationId.HasValue);
        }
    }
}