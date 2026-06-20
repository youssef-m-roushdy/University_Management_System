using FluentValidation;
using University_Management_System.Application.Dtos.UserDtos;
using University_Management_System.Application.Validations.Common;

namespace University_Management_System.Application.Validations.User
{
    public class UpdateProfilePictureDtoValidator : AbstractValidator<UpdateProfilePictureDto>
    {
        public UpdateProfilePictureDtoValidator()
        {
            RuleFor(x => x.ProfilePicture)
                .ValidImageFile();
        }
    }
}