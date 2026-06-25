using FluentValidation;
using University_Management_System.Application.Dtos.InstructorDtos;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.Instructor
{
    public class AddInstructorToExistingUserDtoValidator : AbstractValidator<AddInstructorToExistingUserDto>
    {
        public AddInstructorToExistingUserDtoValidator()
        {
            // ─── User Email ────────────────────────────────────────────────────
            RuleFor(x => x.UserEmail)
                .ValidEmail();

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);
        }
    }
}