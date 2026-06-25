using FluentValidation;
using University_Management_System.Application.Dtos.InstructorDtos;
using University_Management_System.Application.Validations.Common;
using University_Management_System.Application.Validations.User;

namespace University_Management_System.Application.Validations.Instructor
{
    public class CreateInstructorDtoValidator : AbstractValidator<CreateInstructorDto>
    {
        public CreateInstructorDtoValidator()
        {
            // ─── Inherit User Validation ──────────────────────────────────────
            Include(new CreateUserDtoValidator());

            // ─── Department ──────────────────────────────────────────────────
            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);

        }
    }
}