using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using University_Management_System.Application.Commands.StudentStudyYears;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.StudentStudyYears
{
    public class DeleteStudentStudyYearCommandHandler : IRequestHandler<DeleteStudentStudyYearCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteStudentStudyYearCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteStudentStudyYearCommand request, CancellationToken cancellationToken)
        {
            var enrollment = await _unitOfWork.StudentStudyYears.GetByIdAsync(request.Id);
            if (enrollment == null)
                throw new NotFoundException($"Enrollment with ID '{request.Id}' not found.");

            // Check if there are any registrations for this enrollment
            // This prevents deleting if student has course registrations
            var hasRegistrations = await _unitOfWork.Registrations
                .GetRegistrationCountByStudentAsync(enrollment.StudentId) > 0;

            if (hasRegistrations)
                throw new ValidationException(new List<string> { 
                    "Cannot delete enrollment. Student has course registrations for this study year." 
                });

            await _unitOfWork.StudentStudyYears.DeleteAsync(enrollment);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}