using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;

namespace University_Management_System.Application.Validations.Common
{
    public static class CustomValidators
    {
        // ─── Password Validation ──────────────────────────────────────────────
        public static IRuleBuilderOptions<T, string> StrongPassword<T>(
            this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage(ValidationMessages.Required)
                .MinimumLength(8).WithMessage(ValidationMessages.MinLength)
                .MaximumLength(100).WithMessage(ValidationMessages.MaxLength)
                .Must(BeStrongPassword).WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character");
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

        // ─── Email Validation ──────────────────────────────────────────────────
        public static IRuleBuilderOptions<T, string> ValidEmail<T>(
            this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage(ValidationMessages.Required)
                .EmailAddress().WithMessage(ValidationMessages.InvalidEmail)
                .MaximumLength(256).WithMessage(ValidationMessages.MaxLength);
        }

        // ─── Phone Number Validation ──────────────────────────────────────────
        public static IRuleBuilderOptions<T, string> ValidPhoneNumber<T>(
            this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Matches(@"^\+?[1-9][0-9]{7,14}$")
                .WithMessage(ValidationMessages.InvalidPhone);
        }

        // ─── Name Validation ──────────────────────────────────────────────────
        public static IRuleBuilderOptions<T, string> ValidName<T>(
            this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage(ValidationMessages.Required)
                .MinimumLength(2).WithMessage(ValidationMessages.MinLength)
                .MaximumLength(100).WithMessage(ValidationMessages.MaxLength)
                .Matches(@"^[a-zA-Z\s\-']+$")
                .WithMessage("Name can only contain letters, spaces, hyphens, and apostrophes");
        }

        // ─── File Validation ──────────────────────────────────────────────────
        public static IRuleBuilderOptions<T, IFormFile> ValidImageFile<T>(
            this IRuleBuilder<T, IFormFile> ruleBuilder)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var allowedExtensionsFormatted = string.Join(", ", allowedExtensions);

            return ruleBuilder
                .NotNull().WithMessage(ValidationMessages.Required)
                .Must(file => file != null && file.Length > 0).WithMessage("File is empty")
                .Must(file => file != null && file.Length <= 2 * 1024 * 1024)
                    .WithMessage($"File size must not exceed 2MB")
                .Must(file => file == null || allowedExtensions
                    .Contains(Path.GetExtension(file.FileName).ToLowerInvariant()))
                    .WithMessage($"Only {allowedExtensionsFormatted} files are allowed");
        }

        // ─── Role Name Validation ─────────────────────────────────────────────
        public static IRuleBuilderOptions<T, string> ValidRoleName<T>(
            this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage(ValidationMessages.Required)
                .MinimumLength(2).WithMessage(ValidationMessages.MinLength)
                .MaximumLength(50).WithMessage(ValidationMessages.MaxLength)
                .Matches(@"^[a-zA-Z\s\-]+$")
                .WithMessage("Role name can only contain letters, spaces, and hyphens");
        }

        // ─── URL Validation ────────────────────────────────────────────────────
        public static IRuleBuilderOptions<T, string> ValidUrl<T>(
            this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Must(url => string.IsNullOrEmpty(url) || Uri.TryCreate(url, UriKind.Absolute, out _))
                .WithMessage("Invalid URL format");
        }

        // ─── Date Validation ──────────────────────────────────────────────────
        public static IRuleBuilderOptions<T, DateTime> ValidDate<T>(
            this IRuleBuilder<T, DateTime> ruleBuilder)
        {
            return ruleBuilder
                .Must(date => date > DateTime.MinValue && date < DateTime.MaxValue)
                .WithMessage("Invalid date");
        }

        // ─── Future Date Validation ───────────────────────────────────────────
        public static IRuleBuilderOptions<T, DateTime> FutureDate<T>(
            this IRuleBuilder<T, DateTime> ruleBuilder)
        {
            return ruleBuilder
                .Must(date => date > DateTime.UtcNow)
                .WithMessage("Date must be in the future");
        }

        // ─── Past Date Validation ─────────────────────────────────────────────
        public static IRuleBuilderOptions<T, DateTime> PastDate<T>(
            this IRuleBuilder<T, DateTime> ruleBuilder)
        {
            return ruleBuilder
                .Must(date => date < DateTime.UtcNow)
                .WithMessage("Date must be in the past");
        }

        // ─── GPA Validation ──────────────────────────────────────────────────
        // ✅ FIXED: Null check inside Must() because When() receives the root object
        public static IRuleBuilderOptions<T, decimal?> ValidGpa<T>(
            this IRuleBuilder<T, decimal?> ruleBuilder)
        {
            return ruleBuilder
                .Must(gpa => !gpa.HasValue || (gpa.Value >= 0 && gpa.Value <= 4))
                .WithMessage(ValidationMessages.GpaRange);
        }

        // ─── GPA Validation (for non-nullable decimal) ──────────────────────
        public static IRuleBuilderOptions<T, decimal> ValidGpaRequired<T>(
            this IRuleBuilder<T, decimal> ruleBuilder)
        {
            return ruleBuilder
                .InclusiveBetween(0, 4).WithMessage(ValidationMessages.GpaRange);
        }

        // ─── Credits Validation ──────────────────────────────────────────────
        // ✅ FIXED: Null check inside Must() for the same reason
        public static IRuleBuilderOptions<T, int?> ValidCredits<T>(
            this IRuleBuilder<T, int?> ruleBuilder)
        {
            return ruleBuilder
                .Must(credits => !credits.HasValue || credits.Value >= 0)
                .WithMessage(ValidationMessages.CreditsPositive);
        }

        // ─── Credits Validation (for non-nullable int) ──────────────────────
        public static IRuleBuilderOptions<T, int> ValidCreditsRequired<T>(
            this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThanOrEqualTo(0).WithMessage(ValidationMessages.CreditsPositive);
        }
    }
}