using FluentValidation;
using University_Management_System.Application.Dtos.AdminDtos;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.Admin
{
    public class AddAdminToExistingUserDtoValidator : AbstractValidator<AddAdminToExistingUserDto>
    {
        public AddAdminToExistingUserDtoValidator()
        {
            // ─── User Email ────────────────────────────────────────────────────
            RuleFor(x => x.UserEmail)
                .ValidEmail();
        }
    }
}