using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using University_Management_System.Application.Dtos.StudentDtos;
using University_Management_System.Domain.Entities.Identity;

namespace University_Management_System.Application.Commands.Students
{
    public class UpdateStudentCommand : IRequest<StudentDto>
    {
        public string Id { get; set; }
        public UpdateStudentDto Dto { get; set; }
    }
}