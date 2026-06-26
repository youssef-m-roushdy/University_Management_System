using FluentValidation;
using University_Management_System.Application.Dtos.DepartmentCourseDtos;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.DepartmentCourse
{
    public class CreateDepartmentCourseBulkDtoValidator : AbstractValidator<CreateDepartmentCourseBulkDto>
    {
        public CreateDepartmentCourseBulkDtoValidator()
        {
            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);

            RuleFor(x => x.CourseIds)
                .NotNull().WithMessage(ValidationMessages.Required)
                .Must(list => list != null && list.Any()).WithMessage("At least one course is required");

            RuleFor(x => x.CourseIds)
                .Must(list => list == null || list.Distinct().Count() == list.Count)
                .WithMessage("Duplicate course IDs are not allowed")
                .When(x => x.CourseIds != null);

            RuleFor(x => x.CourseIds)
                .Must(list => list == null || !list.Contains(0))
                .WithMessage("Invalid course ID")
                .When(x => x.CourseIds != null);

            RuleFor(x => x.Role)
                .IsInEnum().WithMessage(ValidationMessages.InvalidEnumValue);
        }
    }
}