using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using University_Management_System.Application.Commands.Assistants;
using University_Management_System.Application.Dtos.AssistantDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Assistants
{
    public class AddAssistantToExistingUserCommandHandler : IRequestHandler<AddAssistantToExistingUserCommand, AssistantDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public AddAssistantToExistingUserCommandHandler(
            IUnitOfWork unitOfWork,
            UserManager<User> userManager,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<AssistantDto> Handle(AddAssistantToExistingUserCommand request, CancellationToken cancellationToken)
        {
            // ─── 1. Find User by Email ──────────────────────────────────────
            var user = await _userManager.FindByEmailAsync(request.Dto.UserEmail);
            if (user == null)
                throw new NotFoundException($"User with email '{request.Dto.UserEmail}' not found.");

            // ─── 2. Check if User already has an Assistant profile ──────────
            var existingAssistant = await _unitOfWork.Assistants
                .GetAssistantByUserIdAsync(user.Id);

            if (existingAssistant != null)
                throw new ValidationException(new List<string> {
                    $"User '{request.Dto.UserEmail}' already has an Assistant profile."
                });

            // ─── 3. Check if User already has "Assistant" role ──────────────
            var isAssistant = await _userManager.IsInRoleAsync(user, "Assistant");
            if (isAssistant)
                throw new ValidationException(new List<string> {
                    $"User '{request.Dto.UserEmail}' already has the Assistant role."
                });

            // ─── 4. Validate Department ──────────────────────────────────────
            var department = await _unitOfWork.Departments
                .GetByIdAsync(request.Dto.DepartmentId);
            
            if (department == null)
                throw new NotFoundException($"Department with ID '{request.Dto.DepartmentId}' not found.");

            // ─── 5. Create Assistant Profile ──────────────────────────────────
            var assistant = new Assistant
            {
                Id = user.Id,
                DepartmentId = request.Dto.DepartmentId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Assistants.AddAsync(assistant);
            await _unitOfWork.SaveChangesAsync();

            // ─── 6. Assign "Assistant" role to User ──────────────────────────
            var roleResult = await _userManager.AddToRoleAsync(user, "Assistant");
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                throw new Exception($"Failed to assign Assistant role: {errors}");
            }

            // ─── 7. Return mapped DTO ─────────────────────────────────────────
            return _mapper.Map<AssistantDto>(assistant);
        }
    }
}