using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.SemesterDtos;
using MediatR;

namespace University_Management_System.Application.Commands.Semesters
{
    public class CreateStudyYearSemesterCommand : IRequest<SemesterDto>
    {
        public int StudyYearId { get; set; }
        public CreateSemesterDto SemesterDto { get; set; }

    }
}