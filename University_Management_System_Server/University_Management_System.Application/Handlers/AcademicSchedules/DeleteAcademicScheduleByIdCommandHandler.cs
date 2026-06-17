using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Commands.AcademicSchedules;
using University_Management_System.Domain.Contracts;
using MediatR;

namespace University_Management_System.Application.Handlers.AcademicSchedules
{
    public class DeleteAcademicScheduleByIdCommandHandler : IRequestHandler<DeleteAcademicScheduleByIdCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAcademicScheduleByIdCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteAcademicScheduleByIdCommand request, CancellationToken cancellationToken)
        {
            var schedule = await _unitOfWork.AcademicSchedules.GetByIdAsync(request.Id);

            if (schedule is null)
                return false;

            await _unitOfWork.AcademicSchedules.Delete(schedule);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}