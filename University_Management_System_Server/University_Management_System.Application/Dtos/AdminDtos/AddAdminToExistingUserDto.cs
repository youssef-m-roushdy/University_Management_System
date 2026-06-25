using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Application.Dtos.AdminDtos
{
    public class AddAdminToExistingUserDto
    {
        public string UserEmail { get; set; } = string.Empty;
    }
}