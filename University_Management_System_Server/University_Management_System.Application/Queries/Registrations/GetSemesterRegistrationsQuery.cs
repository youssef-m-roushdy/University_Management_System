using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.RegistrationDtos;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Queries.RegistrationQueries;
using MediatR;

namespace University_Management_System.Application.Queries.Registrations
{
    public record GetSemesterRegistrationsQuery : IRequest<(List<RegistrationDto> Data, int TotalCount)>
    {
        public int SemesterId { get; set; }
        public int StudyYearId { get; set; }
        public RegistrationSemesterQueries? RegistrationQuery { get; set; }
    }
}