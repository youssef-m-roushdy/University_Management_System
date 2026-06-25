using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.UserDtos;

namespace University_Management_System.Application.Dtos.AssistantDtos
{
    public class CreateAssistantDto : CreateUserDto
    {
        public int DepartmentId { get; set; }
    }
}