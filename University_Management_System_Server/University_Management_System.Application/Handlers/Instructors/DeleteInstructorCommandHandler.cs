using MediatR;
using Microsoft.AspNetCore.Identity;
using University_Management_System.Application.Commands.Instructors;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Instructors
{
    public class DeleteInstructorCommandHandler : IRequestHandler<DeleteInstructorCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public DeleteInstructorCommandHandler(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<bool> Handle(DeleteInstructorCommand request, CancellationToken cancellationToken)
        {
            // ─── 1. Check if Instructor exists ────────────────────────────────
            var instructor = await _unitOfWork.Instructors
                .GetInstructorByUserIdAsync(request.Id);
            
            if (instructor == null)
                throw new NotFoundException($"Instructor with ID '{request.Id}' not found.");

            // ─── 2. Get User ────────────────────────────────────────────────────
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user == null)
                throw new NotFoundException($"User with ID '{request.Id}' not found.");

            // ─── 3. Check if user has "Instructor" role ──────────────────────
            var isInstructor = await _userManager.IsInRoleAsync(user, "Instructor");
            if (!isInstructor)
                throw new ValidationException(new List<string> {
                    $"User with ID '{request.Id}' does not have the Instructor role."
                });

            // ─── 4. Remove "Instructor" role ──────────────────────────────────
            var removeRoleResult = await _userManager.RemoveFromRoleAsync(user, "Instructor");
            if (!removeRoleResult.Succeeded)
            {
                var errors = string.Join(", ", removeRoleResult.Errors.Select(e => e.Description));
                throw new Exception($"Failed to remove Instructor role: {errors}");
            }

            // ─── 5. Delete Instructor Profile ──────────────────────────────────
            await _unitOfWork.Instructors.DeleteAsync(instructor);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}