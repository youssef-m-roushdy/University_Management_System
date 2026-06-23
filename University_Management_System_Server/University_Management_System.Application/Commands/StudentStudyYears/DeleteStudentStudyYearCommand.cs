using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace University_Management_System.Application.Commands.StudentStudyYears
{
    public class DeleteStudentStudyYearCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}