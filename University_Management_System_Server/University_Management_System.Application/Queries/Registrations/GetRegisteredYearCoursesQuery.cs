using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.RegistrationDtos;
using MediatR;

namespace University_Management_System.Application.Queries.Registrations
{
    public class GetRegisteredYearCoursesQuery : IRequest<List<RegistrationCourseDto>>
    {
        public string StudentId { get; set; }
        public int Year { get; set; }

        public GetRegisteredYearCoursesQuery(string studentId, int year)
        {
            StudentId = studentId;
            Year = year;
        }
    }
}