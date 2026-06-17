using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Commands.Specializations;
using University_Management_System.Domain.Contracts;
using MediatR;

namespace University_Management_System.Application.Handlers.Specializations
{
    public class UpdateSpecializationCommandHandler : IRequestHandler<UpdateSpecializationCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        public UpdateSpecializationCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<int> Handle(UpdateSpecializationCommand request, CancellationToken cancellationToken)
        {
            var specialization = await _unitOfWork.Specializations.GetByIdAsync(request.Id);
            if (specialization == null)
            {
                throw new Exception("Specialization not found");
            }
            specialization.Name = request.UpdateSpecializationDto.Name;
            specialization.Description = request.UpdateSpecializationDto.Description;
            _unitOfWork.Specializations.Update(specialization);
            await _unitOfWork.SaveChangesAsync();
            return specialization.Id;
        }
    }
}