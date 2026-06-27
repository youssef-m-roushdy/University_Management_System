using FluentValidation;
using Microsoft.AspNetCore.Http;
using University_Management_System.Application.Dtos.AcademicScheduleDtos;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.AcademicSchedule
{
    public class CreateAcademicScheduleDtoValidator : AbstractValidator<CreateAcademicScheduleDto>
    {
        public CreateAcademicScheduleDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage(ValidationMessages.Required)
                .MaximumLength(200).WithMessage(ValidationMessages.MaxLength);

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage(ValidationMessages.MaxLength)
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);

            RuleFor(x => x.SemesterId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);

            RuleFor(x => x.StudyYearId)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidId);

            RuleFor(x => x.ScheduleDate)
                .Must(date => date >= DateTime.UtcNow.Date)
                .WithMessage("Schedule date cannot be in the past");

            RuleFor(x => x.File)
                .Must(file => file == null || file.Length <= 10 * 1024 * 1024) // 10MB
                .WithMessage("File size must not exceed 10MB")
                .When(x => x.File != null);

            RuleFor(x => x.File)
                .Must(file => file == null || 
                    new[] { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".jpg", ".jpeg", ".png" }
                    .Contains(Path.GetExtension(file.FileName).ToLowerInvariant()))
                .WithMessage("Only PDF, Word, Excel, and image files are allowed")
                .When(x => x.File != null);
        }
    }
}