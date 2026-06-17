using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.CourseDtos;
using MediatR;

namespace University_Management_System.Application.Queries.Courses
{
    public class GetStudentRegistrationCoursesQuery : IRequest<IEnumerable<CourseDto>>
    {
        public string UserId { get; set; } = null!;
        public int StudyYearId { get; set; }
        public int SemesterId { get; set; }

        public GetStudentRegistrationCoursesQuery(string userId, int studyYearId, int semesterId)
        {
            UserId = userId;
            StudyYearId = studyYearId;
            SemesterId = semesterId;
        }
    }
}