using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.SemesterDtos;
using MediatR;

namespace University_Management_System.Application.Commands.Semesters
{
    public class CreateSemesterCommand : IRequest<int>
    {
        public int StudyYearId { get; set; }
        public CreateSemesterDto SemesterDto { get; set; }

        public CreateSemesterCommand(int studyYearId, CreateSemesterDto semesterDto)
        {
            StudyYearId = studyYearId;
            SemesterDto = semesterDto;
        }
    }
}