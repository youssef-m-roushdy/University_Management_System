using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using University_Management_System.Application.Dtos.DepartmentDtos.FeeDtos;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Queries;

namespace University_Management_System.Application.Queries.StudyYears
{
    public class GetStudyYearFeesQuery : IRequest<(IEnumerable<FeeDto> Data, int TotalCount)>
    {
        public int StudyYearId { get; set; }
        public GetStudyYearNestedQueries Query { get; set; } = new GetStudyYearNestedQueries();
    }
}