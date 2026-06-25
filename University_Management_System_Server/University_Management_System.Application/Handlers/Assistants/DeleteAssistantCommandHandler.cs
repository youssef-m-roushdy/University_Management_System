using MediatR;
using Microsoft.AspNetCore.Identity;
using University_Management_System.Application.Commands.Assistants;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Assistants
{
    public class DeleteAssistantCommandHandler : IRequestHandler<DeleteAssistantCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public DeleteAssistantCommandHandler(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<bool> Handle(DeleteAssistantCommand request, CancellationToken cancellationToken)
        {
            // ─── 1. Check if Assistant exists ────────────────────────────────
            var assistant = await _unitOfWork.Assistants
                .GetAssistantByUserIdAsync(request.Id);
            
            if (assistant == null)
                throw new NotFoundException($"Assistant with ID '{request.Id}' not found.");

            // ─── 2. Get User ────────────────────────────────────────────────────
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user == null)
                throw new NotFoundException($"User with ID '{request.Id}' not found.");

            // ─── 3. Check if user has "Assistant" role ──────────────────────
            var isAssistant = await _userManager.IsInRoleAsync(user, "Assistant");
            if (!isAssistant)
                throw new ValidationException(new List<string> {
                    $"User with ID '{request.Id}' does not have the Assistant role."
                });

            // ─── 4. Remove "Assistant" role ──────────────────────────────────
            var removeRoleResult = await _userManager.RemoveFromRoleAsync(user, "Assistant");
            if (!removeRoleResult.Succeeded)
            {
                var errors = string.Join(", ", removeRoleResult.Errors.Select(e => e.Description));
                throw new Exception($"Failed to remove Assistant role: {errors}");
            }

            // ─── 5. Delete Assistant Profile ──────────────────────────────────
            await _unitOfWork.Assistants.DeleteAsync(assistant);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}