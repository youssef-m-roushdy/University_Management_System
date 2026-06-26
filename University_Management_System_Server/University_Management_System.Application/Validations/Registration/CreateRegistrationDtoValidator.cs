using FluentValidation;
using University_Management_System.Application.Dtos.RegistrationDtos;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.Registration
{
    public class CreateRegistrationDtoValidator : AbstractValidator<CreateRegistrationDto>
    {
        public CreateRegistrationDtoValidator()
        {
            RuleFor(x => x.CourseId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);

            RuleFor(x => x.SemesterId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);

            RuleFor(x => x.StudyYearId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);
        }
    }
}