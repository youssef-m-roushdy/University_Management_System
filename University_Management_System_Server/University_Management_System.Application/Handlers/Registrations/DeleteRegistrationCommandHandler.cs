using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Commands.Registrations;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;
using MediatR;
using University_Management_System.Shared.Responses;

namespace University_Management_System.Application.Handlers.Registrations
{
    public class DeleteRegistrationCommandHandler : IRequestHandler<DeleteRegistrationCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteRegistrationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<Unit> Handle(DeleteRegistrationCommand request, CancellationToken cancellationToken)
        {
            var registration = await _unitOfWork.Registrations.GetByIdAsync(request.RegistrationId);
            if (registration == null)
           
                throw new NotFoundException("Registration not found");
            

            _unitOfWork.Registrations.Delete(registration);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}