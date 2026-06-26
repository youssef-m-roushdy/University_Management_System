using FluentValidation;
using University_Management_System.Application.Dtos.DepartmentCourseDtos;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.DepartmentCourse
{
    public class UpdateDepartmentCourseDtoValidator : AbstractValidator<UpdateDepartmentCourseDto>
    {
        public UpdateDepartmentCourseDtoValidator()
        {
            RuleFor(x => x.Role)
                .IsInEnum().WithMessage(ValidationMessages.InvalidEnumValue)
                .When(x => x.Role.HasValue);

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(ValidationMessages.AtLeastOneField);
        }

        private static bool HaveAtLeastOneField(UpdateDepartmentCourseDto dto)
        {
            return dto.Role.HasValue;
        }
    }
}