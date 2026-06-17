using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.DepartmentDtos.FeeDtos;
using MediatR;

namespace University_Management_System.Application.Queries.Fees
{
    public class GetFeesOfStudyYearQuery : IRequest<List<FeeDto>>
    {
        public int StudyYearId { get; set; }

        public GetFeesOfStudyYearQuery(int studyYearId)
        {
            StudyYearId = studyYearId;
        }
    }
}