using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace University_Management_System.Application.Commands.StudentStudyYears
{
    public class PromoteSpecificStudentsCommand : IRequest<Unit>
    {
        public List<string> AcademicCodes { get; set; } = new List<string>();

        public PromoteSpecificStudentsCommand(List<string> academicCodes)
        {
            AcademicCodes = academicCodes;
        }
    }
}