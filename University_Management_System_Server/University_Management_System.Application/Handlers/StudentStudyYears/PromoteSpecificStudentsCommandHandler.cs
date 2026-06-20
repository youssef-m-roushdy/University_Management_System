using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Commands.StudentStudyYears;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace University_Management_System.Application.Handlers.StudentStudyYears
{
    public class PromoteSpecificStudentsCommandHandler : IRequestHandler<PromoteSpecificStudentsCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;


        public PromoteSpecificStudentsCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(PromoteSpecificStudentsCommand request, CancellationToken cancellationToken)
        {
            var SpecificUngraduatedStudents = await _unitOfWork.Students.GetSpecificUngraduatedStudentsAsync(request.AcademicCodes);

            if (SpecificUngraduatedStudents.Count() == 0)
                throw new Exception("No ungraduated students found");

            var currentStudyYear = await _unitOfWork.StudyYears.GetCurrentStudyYearAsync();
            if (currentStudyYear == null)
                throw new Exception("Current study year not found");

            var StudentsStudyYears = new List<StudentStudyYear>();
            foreach (var student in SpecificUngraduatedStudents)
            {
                // Re-assign student to the current study year keeping their existing level
                StudentsStudyYears.Add(new StudentStudyYear
                {
                    StudentId = student.Id,
                    StudyYearId = currentStudyYear.Id,
                    Level = student.Level,
                    EnrolledAt = DateTime.UtcNow
                });
            }

            await _unitOfWork.StudentStudyYears.AddRangeAsync(StudentsStudyYears);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}