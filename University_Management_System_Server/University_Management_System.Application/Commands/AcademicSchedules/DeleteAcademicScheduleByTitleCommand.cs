using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace University_Management_System.Application.Commands.AcademicSchedules
{
    public class DeleteAcademicScheduleByTitleCommand : IRequest<bool>
    {
        public string ScheduleTitle { get; set; }

        public DeleteAcademicScheduleByTitleCommand(string scheduleTitle)
        {
            ScheduleTitle = scheduleTitle;
        }
    }
}