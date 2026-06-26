using FluentValidation;
using University_Management_System.Application.Dtos.CoursePrerequisiteDtos;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.CoursePrerequisite
{
    public class CreateCourseDependencyDtoValidator : AbstractValidator<CreateCourseDependencyDto>
    {
        public CreateCourseDependencyDtoValidator()
        {
            RuleFor(x => x.CourseId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);

            RuleFor(x => x.PrerequisiteCourseId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);

            RuleFor(x => x)
                .Must(x => x.CourseId != x.PrerequisiteCourseId)
                .WithMessage("A course cannot depend on itself.");
        }
    }
}