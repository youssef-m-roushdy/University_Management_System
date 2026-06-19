using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using University_Management_System.Shared.Responses;

namespace University_Management_System.Application.Commands.Registrations
{
    public class DeleteRegistrationCommand : IRequest<Unit>
    {
        public int RegistrationId { get; set; }

        public DeleteRegistrationCommand(int registrationId)
        {
            RegistrationId = registrationId;
        }
    }
}