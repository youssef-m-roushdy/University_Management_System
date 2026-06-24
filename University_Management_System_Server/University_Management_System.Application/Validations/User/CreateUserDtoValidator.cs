using FluentValidation;
using University_Management_System.Application.Dtos.UserDtos;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.User
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            // ─── Email ─────────────────────────────────────────────────────────
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(ValidationMessages.Required)
                .EmailAddress().WithMessage(ValidationMessages.InvalidEmail)
                .MaximumLength(256).WithMessage(ValidationMessages.MaxLength);

            // ─── Password ──────────────────────────────────────────────────────
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(ValidationMessages.Required)
                .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                .MaximumLength(100).WithMessage("Password must not exceed 100 characters")
                .Must(BeStrongPassword).WithMessage(
                    "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character");

            // ─── Name ──────────────────────────────────────────────────────────
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(ValidationMessages.Required)
                .MinimumLength(2).WithMessage("Name must be at least 2 characters")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters")
                .Matches(@"^[a-zA-Z\s\-']+$").WithMessage("Name can only contain letters, spaces, hyphens, and apostrophes");

            // ─── UserName ──────────────────────────────────────────────────────
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage(ValidationMessages.Required)
                .MinimumLength(3).WithMessage("Username must be at least 3 characters")
                .MaximumLength(50).WithMessage("Username must not exceed 50 characters")
                .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Username can only contain letters, numbers, and underscores");

            // ─── Phone Number ─────────────────────────────────────────────────
            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?[1-9][0-9]{7,14}$")
                .WithMessage(ValidationMessages.InvalidPhone)
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

            // ─── Address ──────────────────────────────────────────────────────
            RuleFor(x => x.Address)
                .MaximumLength(200).WithMessage("Address must not exceed 200 characters")
                .When(x => !string.IsNullOrEmpty(x.Address));

            // ─── Gender ──────────────────────────────────────────────────────
            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage(ValidationMessages.InvalidEnumValue);
        }

        private static bool BeStrongPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return false;

            var hasUpper = password.Any(char.IsUpper);
            var hasLower = password.Any(char.IsLower);
            var hasDigit = password.Any(char.IsDigit);
            var hasSpecial = password.Any(ch => !char.IsLetterOrDigit(ch));

            return hasUpper && hasLower && hasDigit && hasSpecial;
        }
    }
}