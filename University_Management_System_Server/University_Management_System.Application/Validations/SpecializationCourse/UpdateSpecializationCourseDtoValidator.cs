using FluentValidation;
using University_Management_System.Application.Dtos.SpecializationCourseDtos;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.SpecializationCourse
{
    public class UpdateSpecializationCourseDtoValidator : AbstractValidator<UpdateSpecializationCourseDto>
    {
        public UpdateSpecializationCourseDtoValidator()
        {
            RuleFor(x => x.Role)
                .IsInEnum().WithMessage(ValidationMessages.InvalidEnumValue)
                .When(x => x.Role.HasValue);

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(ValidationMessages.AtLeastOneField);
        }

        private static bool HaveAtLeastOneField(UpdateSpecializationCourseDto dto)
        {
            return dto.Role.HasValue;
        }
    }
}