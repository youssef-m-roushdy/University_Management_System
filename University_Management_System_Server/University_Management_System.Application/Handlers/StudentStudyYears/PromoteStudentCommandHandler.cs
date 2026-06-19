using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Commands.StudentStudyYears;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Domain.Enums;
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Application.Handlers.StudentStudyYears
{
    public class PromoteStudentCommandHandler : IRequestHandler<PromoteStudentCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Student> _userManager;

        public PromoteStudentCommandHandler(IUnitOfWork unitOfWork, UserManager<Student> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<Unit> Handle(PromoteStudentCommand request, CancellationToken cancellationToken)
        {
            var Student = await _userManager.Users.FirstOrDefaultAsync(u => u.AcademicCode == request.AcademicCode);

            if (Student == null)
                throw new Exception("Student not found");

            if (Student.Level == Levels.Graduate)
                throw new Exception("Student has already graduated");

            var currentStudyYear = await _unitOfWork.StudyYears.GetCurrentStudyYearAsync();

            if (currentStudyYear == null)
                throw new Exception("Current study year not found");

            var StudentStudyYear = await _unitOfWork.StudentStudyYears.GetByStudentAndStudyYearAsync(Student.Id, currentStudyYear.Id);

            if (StudentStudyYear != null)
                throw new Exception("Student is already assigned to the current study year");

            // Assign student to current study year with their existing level
            var newStudentStudyYear = new StudentStudyYear
            {
                StudentId = Student.Id,
                StudyYearId = currentStudyYear.Id,
                Level = Student.Level, // now keep level because student now is independent from the user model and can be promoted without changing the user level
                EnrolledAt = DateTime.UtcNow
            };

            await _unitOfWork.StudentStudyYears.AddRangeAsync(new List<StudentStudyYear> { newStudentStudyYear });
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}