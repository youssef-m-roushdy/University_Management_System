using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.RegistrationDtos;
using MediatR;

namespace University_Management_System.Application.Queries.Registrations
{
    public class GetRegisteredCoursesQuery : IRequest<List<RegistrationCourseDto>>
    {
        public string StudentId { get; set; }

        public GetRegisteredCoursesQuery(string studentId)
        {
            StudentId = studentId;
        }
    }
}