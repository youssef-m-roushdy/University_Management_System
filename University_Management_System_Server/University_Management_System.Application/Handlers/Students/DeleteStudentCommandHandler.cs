using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using University_Management_System.Application.Commands.Students;
using University_Management_System.Application.Contracts;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Students
{
    public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public DeleteStudentCommandHandler(
            IUnitOfWork unitOfWork, 
            UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<bool> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            // ─── 1. Check if student exists ──────────────────────────────────
            var student = await _unitOfWork.Students
                .GetByIdAsync(request.Id);

            if (student == null)
                throw new NotFoundException($"Student with ID '{request.Id}' not found.");

            // ─── 2. Get the User (same Id as Student) ──────────────────────
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user == null)
                throw new NotFoundException($"User with ID '{request.Id}' not found.");

            // ─── 3. Check if user has "Student" role ────────────────────────
            var isStudent = await _userManager.IsInRoleAsync(user, "Student");
            if (!isStudent)
                throw new ValidationException(new List<string> {
                    $"User with ID '{request.Id}' does not have the Student role."
                });

            // ─── 4. Remove "Student" role from User ──────────────────────────
            var removeRoleResult = await _userManager.RemoveFromRoleAsync(user, "Student");
            if (!removeRoleResult.Succeeded)
            {
                var errors = string.Join(", ", removeRoleResult.Errors.Select(e => e.Description));
                throw new Exception($"Failed to remove Student role: {errors}");
            }

            // ─── 5. Delete Student Profile ────────────────────────────────────
            await _unitOfWork.Students.DeleteAsync(student);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}