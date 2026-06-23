using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using University_Management_System.Application.Dtos.StudentStudyYearDtos;

namespace University_Management_System.Application.Queries.StudentStudyYears
{
    public class GetStudentCurrentStudyYearQuery : IRequest<StudentStudyYearDto?>
    {
        public string StudentId { get; set; } = string.Empty;
    }
}