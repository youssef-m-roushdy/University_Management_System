using FluentValidation;
using University_Management_System.Application.Dtos.AdminDtos;
using University_Management_System.Application.Validations.Common;
using University_Management_System.Application.Validations.User;

namespace University_Management_System.Application.Validations.Admin
{
    public class CreateAdminDtoValidator : AbstractValidator<CreateAdminDto>
    {
        public CreateAdminDtoValidator()
        {
            // ─── Inherit User Validation ──────────────────────────────────────
            Include(new CreateUserDtoValidator());

            // ─── Admin-specific validations (if any) ─────────────────────────
            // Currently Admin has no additional properties, but this can be extended later
        }
    }
}