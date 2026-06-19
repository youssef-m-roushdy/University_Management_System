using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.CourseDtos;
using MediatR;
using University_Management_System.Shared.Responses;

namespace University_Management_System.Application.Queries.Courses
{
    public record GetCourseYearRegistrationsQuery(int CourseId, int YearId) : IRequest<ApiResponse<CourseWithRegistrationsDto>>;
}
