using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.RegistrationDtos;
using MediatR;
using University_Management_System.Shared.Responses;

namespace University_Management_System.Application.Commands.Registrations
{
    public class UpdateRegistrationCommand : IRequest<Unit>
    {
        public int RegistrationId { get; set; }
        public UpdateRegistrationDto UpdateDto { get; set; } = null!;

        public UpdateRegistrationCommand(int registrationId, UpdateRegistrationDto updateDto)
        {
            RegistrationId = registrationId;
            UpdateDto = updateDto;
        }
    }
}