using FluentValidation;
using University_Management_System.Application.Dtos.CourseDtos;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.Course
{
    public class CreateCourseDtoValidator : AbstractValidator<CreateCourseDto>
    {
        public CreateCourseDtoValidator()
        {
            // ─── Code ──────────────────────────────────────────────────────────
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(ValidationMessages.Required)
                .MaximumLength(20).WithMessage(ValidationMessages.MaxLength)
                .Matches(@"^[A-Za-z0-9\-]+$").WithMessage("Course code can only contain letters, numbers, and hyphens");

            // ─── Name ──────────────────────────────────────────────────────────
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(ValidationMessages.Required)
                .MaximumLength(200).WithMessage(ValidationMessages.MaxLength);

            // ─── Description ───────────────────────────────────────────────────
            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage(ValidationMessages.MaxLength)
                .When(x => !string.IsNullOrEmpty(x.Description));

            // ─── Credits ──────────────────────────────────────────────────────
            RuleFor(x => x.Credits)
                .GreaterThan(0).WithMessage("Credits must be greater than 0")
                .LessThanOrEqualTo(10).WithMessage("Credits must not exceed 10");

            // ─── Status ──────────────────────────────────────────────────────
            RuleFor(x => x.Status)
                .IsInEnum().WithMessage(ValidationMessages.InvalidEnumValue);

            // ─── Department ──────────────────────────────────────────────────
            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);
        }
    }
}