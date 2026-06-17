using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.DepartmentDtos.FeeDtos;
using MediatR;

namespace University_Management_System.Application.Queries.Fees
{
    public class GetFeesOfDepartmentForStudyYearQuery : IRequest<List<FeeDto>>
    {
        public int DepartmentId { get; set; }
        public int StudyYearId { get; set; }

        public GetFeesOfDepartmentForStudyYearQuery(int departmentId, int studyYearId)
        {
            DepartmentId = departmentId;
            StudyYearId = studyYearId;
        }
    }
}