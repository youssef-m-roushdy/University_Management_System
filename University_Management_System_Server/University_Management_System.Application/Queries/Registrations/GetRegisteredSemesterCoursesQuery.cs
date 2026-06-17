using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.RegistrationDtos;
using MediatR;

namespace University_Management_System.Application.Queries.Registrations
{
    public class GetRegisteredSemesterCoursesQuery : IRequest<List<RegistrationCourseDto>>
    {
        public int StudyYearId { get; set; }
        public int SemesterId { get; set; }
        public string StudentId { get; set; }

        public GetRegisteredSemesterCoursesQuery(int studyYearId, int semesterId, string studentId)
        {
            StudyYearId = studyYearId;
            SemesterId = semesterId;
            StudentId = studentId;
        }
    }
}