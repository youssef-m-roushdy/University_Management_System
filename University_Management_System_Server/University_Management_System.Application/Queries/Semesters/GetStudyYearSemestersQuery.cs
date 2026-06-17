using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.SemesterDtos;
using MediatR;

namespace University_Management_System.Application.Queries.Semesters
{
    public class GetStudyYearSemestersQuery : IRequest<List<SemesterDto>>
    {
        public int StudyYearId { get; set; }

        public GetStudyYearSemestersQuery(int studyYearId)
        {
            StudyYearId = studyYearId;
        }
    }
}