using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Application.Dtos.AssistantDtos
{
    public class AddAssistantToExistingUserDto
    {
        public string UserEmail { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
    }
}