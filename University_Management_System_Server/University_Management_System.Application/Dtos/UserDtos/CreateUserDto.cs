using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Dtos.UserDtos
{
    public class CreateUserDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public Gender Gender { get; set; }
        public bool IsActive { get; set; } = true;
    }
}