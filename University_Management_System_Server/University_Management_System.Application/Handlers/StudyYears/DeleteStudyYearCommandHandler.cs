using MediatR;
using University_Management_System.Application.Commands.StudyYears;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.StudyYears
{
    public class DeleteStudyYearCommandHandler : IRequestHandler<DeleteStudyYearCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteStudyYearCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteStudyYearCommand request, CancellationToken cancellationToken)
        {
            var studyYear = await _unitOfWork.StudyYears.GetByIdAsync(request.Id);
            if (studyYear == null)
                throw new NotFoundException($"Study year with ID '{request.Id}' not found");

            // Check if there are any registrations
            var hasRegistrations = await _unitOfWork.StudyYears.HasRegistrationsAsync(request.Id);
            if (hasRegistrations)
                throw new ValidationException(new List<string> { "Cannot delete study year with existing registrations" });

            // Check if there are any semesters
            var hasSemesters = await _unitOfWork.StudyYears.HasSemestersAsync(request.Id);
            if (hasSemesters)
                throw new ValidationException(new List<string> { "Cannot delete study year with existing semesters" });

            // Check if there are any fees
            var hasFees = await _unitOfWork.StudyYears.HasFeesAsync(request.Id);
            if (hasFees)
                throw new ValidationException(new List<string> { "Cannot delete study year with existing fees" });

            await _unitOfWork.StudyYears.DeleteAsync(studyYear);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}