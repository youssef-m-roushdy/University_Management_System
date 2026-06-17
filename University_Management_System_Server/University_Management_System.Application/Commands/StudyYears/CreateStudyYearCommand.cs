using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.StudyYearDtos;
using MediatR;

namespace University_Management_System.Application.Commands.StudyYears
{
    public class CreateStudyYearCommand : IRequest<int>
    {
        public CreateStudyYearDto StudyYearDto { get; set; }

        public CreateStudyYearCommand(CreateStudyYearDto studyYearDto)
        {
            StudyYearDto = studyYearDto;
        }
    }
}