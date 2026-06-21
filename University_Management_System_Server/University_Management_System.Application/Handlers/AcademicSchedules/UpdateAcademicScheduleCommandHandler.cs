using University_Management_System.Application.Commands.AcademicSchedules;
using University_Management_System.Application.Contracts;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;
using MediatR;

namespace University_Management_System.Application.Handlers.AcademicSchedules
{
    public class UpdateAcademicScheduleCommandHandler : IRequestHandler<UpdateAcademicScheduleCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICloudinaryService _cloudinaryService;

        public UpdateAcademicScheduleCommandHandler(IUnitOfWork unitOfWork, ICloudinaryService cloudinaryService)
        {
            _unitOfWork = unitOfWork;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<Unit> Handle(UpdateAcademicScheduleCommand request, CancellationToken cancellationToken)
        {
            var schedule = await _unitOfWork.AcademicSchedules.GetByTitleAsync(request.Title);

            if (schedule is null)
                throw new NotFoundException($"Academic schedule '{request.Title}' not found.");

            //upload to cloudflare r2
            schedule.Title = request.Title;
            schedule.Description = request.Description;
            schedule.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.AcademicSchedules.UpdateAsync(schedule);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
