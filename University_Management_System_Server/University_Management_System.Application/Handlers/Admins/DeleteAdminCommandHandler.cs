using MediatR;
using Microsoft.AspNetCore.Identity;
using University_Management_System.Application.Commands.Admins;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Admins
{
    public class DeleteAdminCommandHandler : IRequestHandler<DeleteAdminCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        // ✅ Seeded admin email (from your data seeding)
        private const string SeededAdminEmail = "admin@akhbaracademy.com";

        public DeleteAdminCommandHandler(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<bool> Handle(DeleteAdminCommand request, CancellationToken cancellationToken)
        {
            // ─── 1. Check if Admin exists ─────────────────────────────────────
            var admin = await _unitOfWork.Admins.GetAdminByUserIdAsync(request.Id);
            if (admin == null)
                throw new NotFoundException($"Admin with ID '{request.Id}' not found.");

            // ─── 2. Get User ────────────────────────────────────────────────────
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user == null)
                throw new NotFoundException($"User with ID '{request.Id}' not found.");

            // ─── 3. ✅ Check if trying to delete the seeded admin ─────────────
            if (user.Email?.ToLower() == SeededAdminEmail.ToLower())
            {
                throw new UnauthorizedAccessException(
                    "Cannot delete the system seeded admin account. This account is required for system administration.");
            }

            // ─── 4. Check if user has "Admin" role ────────────────────────────
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (!isAdmin)
                throw new ValidationException(new List<string> {
                    $"User with ID '{request.Id}' does not have the Admin role."
                });

            // ─── 5. Remove "Admin" role ────────────────────────────────────────
            var removeRoleResult = await _userManager.RemoveFromRoleAsync(user, "Admin");
            if (!removeRoleResult.Succeeded)
            {
                var errors = string.Join(", ", removeRoleResult.Errors.Select(e => e.Description));
                throw new Exception($"Failed to remove Admin role: {errors}");
            }

            // ─── 6. Delete Admin Profile ───────────────────────────────────────
            await _unitOfWork.Admins.DeleteAsync(admin);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}