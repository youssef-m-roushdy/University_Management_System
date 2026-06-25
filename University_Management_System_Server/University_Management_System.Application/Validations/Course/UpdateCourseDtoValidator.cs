using FluentValidation;
using University_Management_System.Application.Dtos.CourseDtos;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.Course
{
    public class UpdateCourseDtoValidator : AbstractValidator<UpdateCourseDto>
    {
        public UpdateCourseDtoValidator()
        {
            // ─── Code ──────────────────────────────────────────────────────────
            RuleFor(x => x.Code)
                .MaximumLength(20).WithMessage(ValidationMessages.MaxLength)
                .Matches(@"^[A-Za-z0-9\-]+$").WithMessage("Course code can only contain letters, numbers, and hyphens")
                .When(x => !string.IsNullOrEmpty(x.Code));

            // ─── Name ──────────────────────────────────────────────────────────
            RuleFor(x => x.Name)
                .MaximumLength(200).WithMessage(ValidationMessages.MaxLength)
                .When(x => !string.IsNullOrEmpty(x.Name));

            // ─── Description ───────────────────────────────────────────────────
            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage(ValidationMessages.MaxLength)
                .When(x => !string.IsNullOrEmpty(x.Description));

            // ─── Credits ──────────────────────────────────────────────────────
            RuleFor(x => x.Credits)
                .GreaterThan(0).WithMessage("Credits must be greater than 0")
                .LessThanOrEqualTo(10).WithMessage("Credits must not exceed 10")
                .When(x => x.Credits.HasValue);

            // ─── Status ──────────────────────────────────────────────────────
            RuleFor(x => x.Status)
                .IsInEnum().WithMessage(ValidationMessages.InvalidEnumValue)
                .When(x => x.Status.HasValue);

            // ─── Department ──────────────────────────────────────────────────
            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId)
                .When(x => x.DepartmentId.HasValue);

            // ─── At least one field must be provided ─────────────────────────
            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(ValidationMessages.AtLeastOneField);
        }

        private static bool HaveAtLeastOneField(UpdateCourseDto dto)
        {
            return !string.IsNullOrEmpty(dto.Code) ||
                   !string.IsNullOrEmpty(dto.Name) ||
                   !string.IsNullOrEmpty(dto.Description) ||
                   dto.Credits.HasValue ||
                   dto.Status.HasValue ||
                   dto.DepartmentId.HasValue;
        }
    }
}