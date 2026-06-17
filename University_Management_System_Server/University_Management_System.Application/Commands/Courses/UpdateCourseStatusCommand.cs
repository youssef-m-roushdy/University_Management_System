using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.CourseDtos;
using MediatR;

namespace University_Management_System.Application.Commands.Courses
{
    public class UpdateCourseStatusCommand : IRequest<Unit>
    {
        public UpdateCourseStatusDto Dto { get; set; }

        public UpdateCourseStatusCommand(UpdateCourseStatusDto dto)
        {
            Dto = dto;
        }
    }
}