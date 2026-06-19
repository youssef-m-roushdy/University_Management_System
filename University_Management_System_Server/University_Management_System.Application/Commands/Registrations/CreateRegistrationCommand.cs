using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.RegistrationDtos;
using MediatR;
using University_Management_System.Shared.Responses;

namespace University_Management_System.Application.Commands.Registrations
{
    public class CreateRegistrationCommand : IRequest<int>
    {
        public CreateRegistrationDto RegistrationDto { get; set; } = null!;
        public string UserId { get; set; } = string.Empty;

        public CreateRegistrationCommand(CreateRegistrationDto registrationDto, string userId)
        {
            RegistrationDto = registrationDto;
            UserId = userId;
        }
    }
}