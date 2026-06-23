using FluentValidation;
using University_Management_System.Application.Dtos.StudyYearDtos;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.StudyYear
{
    public class PatchStudyYearDtoValidator : AbstractValidator<PatchStudyYearDto>
    {
        public PatchStudyYearDtoValidator()
        {
            RuleFor(x => x.IsCurrent)
                .NotNull()
                .WithMessage("IsCurrent is required.");
        }
    }
}