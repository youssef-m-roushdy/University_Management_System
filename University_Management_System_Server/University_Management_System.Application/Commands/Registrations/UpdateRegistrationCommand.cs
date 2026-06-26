using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.RegistrationDtos;
using MediatR;
using University_Management_System.Shared.Responses;

namespace University_Management_System.Application.Commands.Registrations
{
    public class UpdateRegistrationCommand : IRequest<RegistrationDto>
    {
        public int Id { get; set; }
        public UpdateRegistrationDto? Dto { get; set; }
        public string StudentId { get; set; }
    }
}