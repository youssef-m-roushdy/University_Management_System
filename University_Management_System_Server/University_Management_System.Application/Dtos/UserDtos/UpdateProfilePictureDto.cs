using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace University_Management_System.Application.Dtos.UserDtos
{
    public class UpdateProfilePictureDto
    {
        public IFormFile ProfilePicture { get; set; } = null!;
    }
}