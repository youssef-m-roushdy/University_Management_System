using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.RegistrationDtos;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Queries;
using MediatR;

namespace University_Management_System.Application.Queries.Registrations
{
    public record GetAllSemesterRegistrationsQuery : IRequest<(List<RegistrationDto> Data, int TotalCount)>
    {
        public int SemesterId { get; set; }
        public int StudyYearId { get; set; }
        public RegistrationQuery? RegistrationQuery { get; set; }
        public GetAllSemesterRegistrationsQuery(int studyYearId, int semesterId, RegistrationQuery? registrationQuery = null)
        {
            SemesterId = semesterId;
            StudyYearId = studyYearId;
            RegistrationQuery = registrationQuery;
        }
    }
}