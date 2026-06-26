using MediatR;
using University_Management_System.Application.Commands.Specializations;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Specializations
{
    public class DeleteSpecializationCommandHandler : IRequestHandler<DeleteSpecializationCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSpecializationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteSpecializationCommand request, CancellationToken cancellationToken)
        {
            // ─── 1. Check if Specialization exists ────────────────────────────
            var specialization = await _unitOfWork.Specializations
                .GetByIdAsync(request.Id);
            
            if (specialization == null)
                throw new NotFoundException($"Specialization with ID '{request.Id}' not found.");

            // ─── 2. Check if students are assigned ────────────────────────────
            var studentCount = await _unitOfWork.Specializations
                .GetStudentCountAsync(request.Id);
            
            if (studentCount > 0)
                throw new ValidationException(new List<string> {
                    $"Cannot delete specialization '{specialization.Name}' because it has {studentCount} students assigned."
                });

            // ─── 3. Delete Specialization ──────────────────────────────────────
            await _unitOfWork.Specializations.DeleteAsync(specialization);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}