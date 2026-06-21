using FluentValidation;
using University_Management_System.Application.Dtos.StudyYearDtos;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.StudyYear
{
    public class UpdateStudyYearDtoValidator : AbstractValidator<UpdateStudyYearDto>
    {
        public UpdateStudyYearDtoValidator()
        {
            RuleFor(x => x.StartYear)
                .GreaterThan(2000).WithMessage("Start year must be greater than 2000")
                .LessThan(2100).WithMessage("Start year must be less than 2100");

            RuleFor(x => x.EndYear)
                .GreaterThan(2000).WithMessage("End year must be greater than 2000")
                .LessThan(2100).WithMessage("End year must be less than 2100");

            RuleFor(x => x)
                .Must(x => x.EndYear > x.StartYear)
                .WithMessage("End year must be greater than start year");

            RuleFor(x => x)
                .Must(x => x.EndYear - x.StartYear <= 1)
                .WithMessage("Study year duration cannot exceed 1 year");
        }
    }
}