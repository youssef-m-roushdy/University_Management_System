using FluentValidation;
using University_Management_System.Application.Commands.DepartmentCourses;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.DepartmentCourse
{
    public class DeleteDepartmentCourseCommandValidator : AbstractValidator<DeleteDepartmentCourseCommand>
    {
        public DeleteDepartmentCourseCommandValidator()
        {
            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);

            RuleFor(x => x.CourseId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);
        }
    }
}