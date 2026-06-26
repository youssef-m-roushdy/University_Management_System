using FluentValidation;
using University_Management_System.Application.Dtos.DepartmentCourseDtos;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.DepartmentCourse
{
    public class CreateDepartmentCourseDtoValidator : AbstractValidator<CreateDepartmentCourseDto>
    {
        public CreateDepartmentCourseDtoValidator()
        {
            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);

            RuleFor(x => x.CourseId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);

            RuleFor(x => x.Role)
                .IsInEnum().WithMessage(ValidationMessages.InvalidEnumValue);
        }
    }
}