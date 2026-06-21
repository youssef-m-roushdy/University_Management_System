using University_Management_System.Application.Commands.AcademicSchedules;
using University_Management_System.Application.Contracts;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;
using MediatR;

namespace University_Management_System.Application.Handlers.AcademicSchedules
{
    public class DeleteAcademicScheduleByTitleCommandHandler : IRequestHandler<DeleteAcademicScheduleByTitleCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICloudinaryService _cloudinaryService;

        public DeleteAcademicScheduleByTitleCommandHandler(IUnitOfWork unitOfWork, ICloudinaryService cloudinaryService)
        {
            _unitOfWork = unitOfWork;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<bool> Handle(DeleteAcademicScheduleByTitleCommand request, CancellationToken cancellationToken)
        {
            var schedule = await _unitOfWork.AcademicSchedules.GetByTitleAsync(request.ScheduleTitle);

            if (schedule is null)
                throw new NotFoundException($"Academic schedule '{request.ScheduleTitle}' not found.");


            await _unitOfWork.AcademicSchedules.DeleteAsync(schedule);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
