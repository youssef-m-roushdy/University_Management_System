using FluentValidation;
using University_Management_System.Application.Dtos.StudyYearDtos;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.StudyYear
{
    public class PatchStudyYearDtoValidator : AbstractValidator<PatchStudyYearDto>
    {
        public PatchStudyYearDtoValidator()
        {
            When(x => x.StartYear.HasValue, () =>
            {
                RuleFor(x => x.StartYear)
                    .GreaterThan(2000).WithMessage("Start year must be greater than 2000")
                    .LessThan(2100).WithMessage("Start year must be less than 2100");
            });

            When(x => x.EndYear.HasValue, () =>
            {
                RuleFor(x => x.EndYear)
                    .GreaterThan(2000).WithMessage("End year must be greater than 2000")
                    .LessThan(2100).WithMessage("End year must be less than 2100");
            });

            When(x => x.StartYear.HasValue && x.EndYear.HasValue, () =>
            {
                RuleFor(x => x)
                    .Must(x => x.EndYear.Value > x.StartYear.Value)
                    .WithMessage("End year must be greater than start year");

                RuleFor(x => x)
                    .Must(x => x.EndYear.Value - x.StartYear.Value <= 1)
                    .WithMessage("Study year duration cannot exceed 1 year");
            });

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(ValidationMessages.AtLeastOneField);
        }

        private static bool HaveAtLeastOneField(PatchStudyYearDto dto)
        {
            return dto.StartYear.HasValue ||
                   dto.EndYear.HasValue ||
                   dto.IsCurrent.HasValue;
        }
    }
}