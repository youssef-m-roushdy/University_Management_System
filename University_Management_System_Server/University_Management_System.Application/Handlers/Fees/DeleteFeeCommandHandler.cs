using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Commands.Fees;
using University_Management_System.Domain.Contracts;
using MediatR;

namespace University_Management_System.Application.Handlers.Fees
{
    public class DeleteFeeCommandHandler : IRequestHandler<DeleteFeeCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteFeeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteFeeCommand request, CancellationToken cancellationToken)
        {
            var fee = await _unitOfWork.Fees.GetByIdAsync(request.Id);
            if (fee is null)
                return Unit.Value; // If fee not found, we can consider it as already deleted and return success.

            await _unitOfWork.Fees.DeleteAsync(fee);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}