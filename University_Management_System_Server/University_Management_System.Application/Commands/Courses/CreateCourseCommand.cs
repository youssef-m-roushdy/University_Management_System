using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.CourseDtos;
using MediatR;
using University_Management_System.Shared.Respones;

namespace University_Management_System.Application.Commands.Courses
{
    public class CreateCourseCommand: IRequest<Response<CourseDto>>
    {
        public CreateCourseDto Course { get; set; }

        public CreateCourseCommand(CreateCourseDto course)
        {
            Course = course;
        }
    }
}