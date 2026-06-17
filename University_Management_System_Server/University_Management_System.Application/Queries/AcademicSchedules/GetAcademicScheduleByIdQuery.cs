using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.AcademicSheduleDtos;
using MediatR;

namespace University_Management_System.Application.Queries.AcademicSchedules
{
    public class GetAcademicScheduleByIdQuery : IRequest<AcademicScheduleDto>
    {
        public int Id { get; init; }

        public GetAcademicScheduleByIdQuery(int id)
        {
            Id = id;
        }
    }
}