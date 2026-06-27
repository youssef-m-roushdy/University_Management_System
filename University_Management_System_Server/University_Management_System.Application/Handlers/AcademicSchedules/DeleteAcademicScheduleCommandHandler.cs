using MediatR;
using University_Management_System.Application.Commands.AcademicSchedules;
using University_Management_System.Application.Contracts;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.AcademicSchedules
{
    public class DeleteAcademicScheduleCommandHandler : IRequestHandler<DeleteAcademicScheduleCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IR2StorageService _r2StorageService;

        public DeleteAcademicScheduleCommandHandler(
            IUnitOfWork unitOfWork,
            IR2StorageService r2StorageService)
        {
            _unitOfWork = unitOfWork;
            _r2StorageService = r2StorageService;
        }

        public async Task<bool> Handle(DeleteAcademicScheduleCommand request, CancellationToken cancellationToken)
        {
            // ─── 1. Get schedule ──────────────────────────────────────────────
            var schedule = await _unitOfWork.AcademicSchedules
                .GetByIdAsync(request.Id);

            if (schedule == null)
                throw new NotFoundException($"Academic Schedule with ID '{request.Id}' not found.");

            // ─── 2. Verify Admin ownership ────────────────────────────────────
            if (schedule.AdminId != request.AdminId)
                throw new UnauthorizedAccessException("You can only delete schedules you created.");

            // ─── 3. Check if schedule is being used ──────────────────────────
            // Optional: Check if any other entities reference this schedule
            // For example, if you have a ScheduleReference table:
            // var hasReferences = await _unitOfWork.ScheduleReferences
            //     .AnyAsync(sr => sr.ScheduleId == request.Id);
            // 
            // if (hasReferences)
            //     throw new ValidationException(new List<string> {
            //         "Cannot delete schedule because it is being referenced by other records."
            //     });

            // ─── 4. Delete file from R2 (if exists) ──────────────────────────
            if (!string.IsNullOrEmpty(schedule.Url))
            {
                try
                {
                    // Extract the key from the URL
                    var key = _r2StorageService.ExtractKeyFromUrl(schedule.Url);
                    await _r2StorageService.DeleteAsync(key, cancellationToken);
                }
                catch (Exception ex)
                {
                    // Log the error but continue with deletion
                    // _logger.LogWarning(ex, "Failed to delete file from R2: {Url}", schedule.Url);
                }
            }

            // ─── 5. Delete schedule ──────────────────────────────────────────
            await _unitOfWork.AcademicSchedules.DeleteAsync(schedule);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        
    }
}