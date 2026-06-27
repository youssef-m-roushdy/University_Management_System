using FluentValidation;
using University_Management_System.Application.Commands.AcademicSchedules;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.AcademicSchedule
{
    public class DeleteAcademicScheduleCommandValidator : AbstractValidator<DeleteAcademicScheduleCommand>
    {
        public DeleteAcademicScheduleCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);

            RuleFor(x => x.AdminId)
                .NotEmpty().WithMessage("Admin ID is required");
        }
    }
}