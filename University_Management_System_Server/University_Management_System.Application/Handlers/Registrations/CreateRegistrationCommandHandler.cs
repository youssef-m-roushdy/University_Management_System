using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Commands.Registrations;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Enums;
using University_Management_System.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using University_Management_System.Shared.Responses;
using University_Management_System.Domain.Contracts;

namespace University_Management_System.Application.Handlers.Registrations
{
    public class CreateRegistrationCommandHandler : IRequestHandler<CreateRegistrationCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public CreateRegistrationCommandHandler(
            IUnitOfWork unitOfWork,
            UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<int> Handle(CreateRegistrationCommand request, CancellationToken cancellationToken)
        {
            // 1️⃣ Get user
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);

            if (user == null)
                throw new NotFoundException("User not found");
            
            // 2️⃣ Check user is student
            if (!await _userManager.IsInRoleAsync(user, "Student"))
                throw new BadRequestException("Only students can register for courses");
            
            // 3️⃣ Get student
            var student = await _unitOfWork.Students.GetByIdAsync(user.Id);

            // 4️⃣ Get course
            var course = await _unitOfWork.Courses.GetByIdAsync(request.RegistrationDto.CourseId);

            if (course == null)
                throw new NotFoundException("Course not found");

            // 5️⃣ Check study year is current
            var studyYear = await _unitOfWork.StudyYears.IsStudyYearCurrentAsync(request.RegistrationDto.StudyYearId);

            if (!studyYear)
                throw new BadRequestException("You can only register in the current study year, this study year is ended");

            // 6️⃣ Check semester belongs to study year
            var isSemesterInStudyYear = await _unitOfWork.Semesters.IsSemesterBelongsToStudyYearAsync(request.RegistrationDto.SemesterId, request.RegistrationDto.StudyYearId);

            if (!isSemesterInStudyYear)
                throw new BadRequestException("The semester does not belong to the specified study year");

            // 7️⃣ Check semester is active
            var semester = await _unitOfWork.Semesters.IsActiveSemesterAsync(request.RegistrationDto.SemesterId);

            if (!semester)
                throw new BadRequestException("You can only register in the active semester, this semester is ended");

            // 8️⃣ Check course is opened
            if (course.Status != CourseStatus.Opened)
                throw new BadRequestException("Course registration is closed");

            // 9️⃣ Check if user is already registered in the course
            var isRegistrationExists = await _unitOfWork.Registrations.IsUserRegisteredInCourseAsync(user.Id, course.Id);

            if (isRegistrationExists)
                throw new BadRequestException("Already registered in this course");

            // Check prerequisites
            var prerequisitesCourses = await _unitOfWork.Courses.GetCoursePrerequisitesAsync(course.Id);

            var passedCourseIds = new List<int>();
            foreach (var preq in prerequisitesCourses)
            {
                var isPassed = await _unitOfWork.Registrations.IsCourseCompletedByUserAsync(user.Id, preq.Id);
                if (isPassed)
                {
                    passedCourseIds.Add(preq.Id);
                }
            }

            var missingCourses = prerequisitesCourses.Select(p => p.Id).Except(passedCourseIds);

            if (missingCourses.Any())
                throw new BadRequestException("Prerequisites not completed");

            // Check if student has enough credit hours
            if (student.AllowedCredits < course.Credits)
                throw new BadRequestException("Not enough credit hours");

            // 1️⃣0️⃣ Create registration
            var registration = new Registration
            {
                StudentId = user.Id,
                CourseId = course.Id,
                StudyYearId = request.RegistrationDto.StudyYearId,
                SemesterId = request.RegistrationDto.SemesterId,
                Status = RegistrationStatus.Pending,
                Progress = CourseProgress.NotStarted,
                Grade = null,
                IsPassed = false,
                RegisteredAt = DateTime.UtcNow
            };

            await _unitOfWork.Registrations.AddAsync(registration);
            await _unitOfWork.SaveChangesAsync();

            // 1️⃣1️⃣ Deduct credit hours
            student.AllowedCredits -= course.Credits;
            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
                throw new BadRequestException("Failed to update student credit hours");

            return registration.Id;
        }
    }
}