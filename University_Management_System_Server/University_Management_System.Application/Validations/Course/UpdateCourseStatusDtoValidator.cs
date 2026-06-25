using FluentValidation;
using University_Management_System.Application.Dtos.CourseDtos;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.Course
{
    public class UpdateCourseStatusDtoValidator : AbstractValidator<UpdateCourseStatusDto>
    {
        public UpdateCourseStatusDtoValidator()
        {
            RuleFor(x => x.Status)
                .IsInEnum().WithMessage(ValidationMessages.InvalidEnumValue);
        }
    }
}