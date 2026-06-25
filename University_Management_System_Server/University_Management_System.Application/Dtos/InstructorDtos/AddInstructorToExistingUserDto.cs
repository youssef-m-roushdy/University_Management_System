using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Application.Dtos.InstructorDtos
{
    public class AddInstructorToExistingUserDto
    {
        public string UserEmail { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
    }
}