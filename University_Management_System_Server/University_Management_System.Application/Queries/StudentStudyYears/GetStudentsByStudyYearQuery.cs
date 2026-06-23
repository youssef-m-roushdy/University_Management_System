using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using University_Management_System.Application.Dtos.StudentStudyYearDtos;
using University_Management_System.Domain.Queries;

namespace University_Management_System.Application.Queries.StudentStudyYears
{
    public class GetStudentsByStudyYearQuery : IRequest<(IEnumerable<StudentStudyYearDto> Data, int TotalCount)>
    {
        public int StudyYearId { get; set; }
        public StudyYearStudentQueries Query { get; set; } = new StudyYearStudentQueries();
    }
}