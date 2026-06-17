using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Commands.Registrations;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Enums;
using University_Management_System.Shared.Exceptions;
using MediatR;
using University_Management_System.Shared.Respones;

namespace University_Management_System.Application.Handlers.Registrations
{
    public class UpdateRegistrationCommandHandler : IRequestHandler<UpdateRegistrationCommand,  Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateRegistrationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<Unit> Handle(UpdateRegistrationCommand request, CancellationToken cancellationToken)
        {
            var registration = await _unitOfWork.Registrations.GetByIdAsync(request.RegistrationId);
            if (registration == null)                
                throw new NotFoundException("Registration not found");

            var studyYear = await _unitOfWork.StudyYears.GetCurrentStudyYearAsync();
            if (studyYear.Id != registration.StudyYearId)
                throw new Exception($"Cannot update registration for a non current study year.");
            
            

            var semester = await _unitOfWork.Semesters.GetByIdAsync(registration.SemesterId);
            if(!semester.IsActive)
                throw new Exception($"Cannot update registration for an inactive semester.");

            registration.Status = request.UpdateDto.Status;
            registration.Reason = request.UpdateDto.Reason;
            registration.Grade = request.UpdateDto.Grade;
            if (registration.Status == RegistrationStatus.Approved)
            {
                registration.Progress = CourseProgress.InProgress;
            }
            else
            {
                registration.Progress = CourseProgress.NotStarted;
            }

            if(registration.Grade != null)
            {
                if(registration.Grade == Grades.F)
                {
                    registration.IsPassed = false;
                    registration.Progress = CourseProgress.Completed; // Completed but not passed
                }
                else
                {
                    registration.IsPassed = true;
                    registration.Progress = CourseProgress.Completed; // Completed and passed
                }
            }

            _unitOfWork.Registrations.Update(registration);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}