using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.AcademicSheduleDtos;
using MediatR;

namespace University_Management_System.Application.Queries.AcademicSchedules
{
    public class GetAcademicSchedulesBySemesterIdQuery : IRequest<IEnumerable<AcademicScheduleDto>>
     {
        public int SemesterId { get; set; }

        public GetAcademicSchedulesBySemesterIdQuery(int semesterId)
        {
            SemesterId = semesterId;
        }
     }
}