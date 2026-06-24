using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using University_Management_System.Application.Dtos.StudentDtos;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Commands.Students
{
    public class CreateStudentCommand : IRequest<StudentDto>
    {
        public CreateStudentDto Dto { get; set; }
    }
}