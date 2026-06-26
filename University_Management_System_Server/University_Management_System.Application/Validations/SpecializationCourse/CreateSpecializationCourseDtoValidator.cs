using FluentValidation;
using University_Management_System.Application.Dtos.SpecializationCourseDtos;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.SpecializationCourse
{
    public class CreateSpecializationCourseDtoValidator : AbstractValidator<CreateSpecializationCourseDto>
    {
        public CreateSpecializationCourseDtoValidator()
        {
            RuleFor(x => x.SpecializationId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);

            RuleFor(x => x.CourseId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);

            RuleFor(x => x.Role)
                .IsInEnum().WithMessage(ValidationMessages.InvalidEnumValue);
        }
    }
}